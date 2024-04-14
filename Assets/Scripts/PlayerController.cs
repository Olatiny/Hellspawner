using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 6;
    public int currentHealth;

    [Header("Platforming")]
    [SerializeField] private float gravity = 75f;
    [SerializeField] private float jumpVelocity = 15f;
    [SerializeField] private float jumpHoldTime = .15f;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float jumpBufferTime = .1f;
    [SerializeField] private float jumpRaycastDist = .8f;

    [Header("Attacking/Projectiles")]
    [SerializeField] private PlayerProjectile projectilePrefab;
    [SerializeField] private float projectileSpawnDist = .5f;
    [SerializeField] private int maxProjectiles = 3;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private int defaultAttackDamage = 1;
    [SerializeField] private int chargeAttackDamage = 3;
    [SerializeField] private float maxChargeTime = 1.5f;
    [SerializeField] private float attackCooldownTime = .1f;
    public List<PlayerProjectile> projectiles;
    public float currentAttackDamage;

    [Header("Dashing")]
    [SerializeField] private float dashSpeed = 1f;
    [SerializeField] private float dashTime = .25f;

    [Header("Frost")]
    [SerializeField] private FrostAOE frostAOEprefab;
    [SerializeField] private float frostTime = 2f;
    [SerializeField] private float frostCooldown = 4f;
    [SerializeField] private float slowDownTime = 2f;

    private GameManager gameManagerRef;
    public PlayerInput playerInput;
    private PlayerInputActions inputActions;
    private Rigidbody2D myRigidBody;
    private Collider2D myCollider;
    private Vector2 direction;
    private Animator myAnimator;

    bool grounded = true;
    bool holdingJump = false;
    bool bufferingJump = false;
    bool attackCooldown = false;
    bool dashing = false;
    bool canDash = true;
    bool canSpawnFrost = true;
    bool slowed = false;

    Coroutine jumpBufferRoutine = null;
    Coroutine jumpHoldRoutine = null;
    Coroutine chargeAttackRoutine = null;

    private void Start()
    {
        projectiles = new();
        currentHealth = maxHealth;

        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        gameManagerRef = GameManager.Instance;
        myAnimator = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();
        inputActions = new();
        inputActions.PlatformerControls.Enable();

        inputActions.PlatformerControls.Jump.performed += JumpInput;
        inputActions.PlatformerControls.Jump.canceled += JumpInput;

        inputActions.PlatformerControls.Attack.performed += Attack;
        inputActions.PlatformerControls.Attack.canceled += Attack;

        inputActions.PlatformerControls.Dash.performed += Dash;

        inputActions.PlatformerControls.Frost.performed += Frost;

        direction = Vector2.right;
    }

    private void FixedUpdate()
    {
        if( gameManagerRef.paused )
            return;

        if (direction.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

        if (dashing)
            return;

        // Gravity if not jumping
        if (!holdingJump && myRigidBody.velocity.y > -maxFallSpeed)
            myRigidBody.velocity += (slowed ? gravity / 2 : gravity) * Time.fixedDeltaTime * Vector2.down;

        // Check if Grounded 
        RaycastHit2D hit = Physics2D.CircleCast(gameObject.transform.position, .25f, Vector2.down, jumpRaycastDist);
        //(gameObject.transform.position, Vector2.down * 1.5f, Color.black, .05f);
        grounded = hit.collider != null && hit.collider != myCollider;

        if (grounded)
            canDash = true;

        // Buffer Jump
        if (grounded & bufferingJump)
        {
            StopCoroutine(jumpBufferRoutine);
            jumpBufferRoutine = null;
            Jump();
        }

        // Movement
        Vector2 moveInput = inputActions.PlatformerControls.Move.ReadValue<Vector2>();

        if (moveInput.x != 0)
        {
            direction = new(moveInput.x / Mathf.Abs(moveInput.x), 0);
            myRigidBody.velocity = new(direction.x * walkSpeed, myRigidBody.velocity.y);
        }
        else
            myRigidBody.velocity = new(0, myRigidBody.velocity.y);

        if (slowed)
            myRigidBody.velocity = new(myRigidBody.velocity.x / 2, myRigidBody.velocity.y);

        myAnimator.SetBool("Airborne", !grounded);
        myAnimator.SetBool("Running", grounded && myRigidBody.velocity.x != 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FrostAOE") && collision.gameObject.GetComponent<FrostAOE>().owner != gameObject)
            StartCoroutine(SlowDown(slowDownTime));
    }

    public void TakeDamage(int damageAmt)
    {
        if (!dashing)
            gameManagerRef.PlayerTakeDamage(damageAmt);
    }

    public void JumpInput(CallbackContext context)
    {
        if (context.performed && !grounded)
        {
            if (jumpBufferRoutine != null)
            {
                StopCoroutine(jumpBufferRoutine);
                jumpBufferRoutine = null;
            }

            jumpBufferRoutine = StartCoroutine(BufferJump(jumpBufferTime));
            return;
        }

        if (context.performed && grounded)
        {
            Jump();
        }
        else if (context.canceled)
        {
            holdingJump = false;

            if (jumpHoldRoutine != null)
            {
                StopCoroutine(jumpHoldRoutine);
                jumpHoldRoutine = null;
            }
        }
    }

    public void Jump()
    {
        if( gameManagerRef.paused )
            return;

        bufferingJump = false;
        holdingJump = true;

        // come back to this if you decide to give player acceleration
        int waveDashMult = dashing ? 2 : 1;

        myRigidBody.velocity = (slowed ? jumpVelocity / 1.4f : jumpVelocity) * Vector2.up;
        //myRigidBody.velocity = new Vector2(myRigidBody.velocity.x * waveDashMult, jumpVelocity);
        jumpHoldRoutine = StartCoroutine(JumpHoldTimer(jumpHoldTime));
    }

    public void Attack(CallbackContext context)
    {
        if( gameManagerRef.paused )
            return;

        if (projectiles.Count >= maxProjectiles || attackCooldown)
            return;

        if (context.performed && gameManagerRef.DemonDefeated)
        {
            chargeAttackRoutine = StartCoroutine(ChargeAttackTimer(maxChargeTime));
        }
        else if (context.canceled)
        {
            myAnimator.SetTrigger("Shoot");
            PlayerProjectile goop = Instantiate(projectilePrefab, transform.position + (Vector3)(direction * projectileSpawnDist), transform.rotation);
            goop.SendProjectile(this, projectileSpeed, direction.normalized, currentAttackDamage);
            projectiles.Add(goop);

            if (chargeAttackRoutine != null)
                StopCoroutine(chargeAttackRoutine);

            StartCoroutine(AttackCooldownTimer(attackCooldownTime));
        }
    }

    public void Dash(CallbackContext context)
    {
        if( gameManagerRef.paused )
            return;

        if (!context.performed || !canDash || !gameManagerRef.LichDefeated)
            return;

        canDash = false;
        dashing = true;

        Vector2 moveInput = inputActions.PlatformerControls.Move.ReadValue<Vector2>();

        if (moveInput == Vector2.zero)
            moveInput = direction;

        myRigidBody.velocity = moveInput.normalized * dashSpeed;
        StartCoroutine(DashTimer(dashTime));
    }

    public void Frost(CallbackContext context)
    {
        if( gameManagerRef.paused )
            return;

        if (context.performed && canSpawnFrost && gameManagerRef.FrostWardenDefeated)
        {
            StartCoroutine(FrostTimer(frostTime, frostCooldown));
        }
    }

    public void Kill()
    {
        inputActions.PlatformerControls.Jump.performed -= JumpInput;
        inputActions.PlatformerControls.Jump.canceled -= JumpInput;

        inputActions.PlatformerControls.Attack.performed -= Attack;
        inputActions.PlatformerControls.Attack.canceled -= Attack;

        inputActions.PlatformerControls.Dash.performed -= Dash;

        inputActions.PlatformerControls.Frost.performed -= Frost;

        inputActions.Dispose();
        Destroy(this);
        Destroy(gameObject);
    }

    IEnumerator JumpHoldTimer(float seconds = 0.5f)
    {
        if (!holdingJump)
            yield return null;

        yield return new WaitForSeconds(seconds);
        holdingJump = false;
        jumpHoldRoutine = null;
    }

    IEnumerator ChargeAttackTimer(float seconds = 1.5f)
    {
        currentAttackDamage = defaultAttackDamage;

        float elapsedTime = 0;

        while (currentAttackDamage < chargeAttackDamage)
        {
            currentAttackDamage = Mathf.Lerp(defaultAttackDamage, chargeAttackDamage, elapsedTime / seconds);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        chargeAttackRoutine = null;
        yield return null;
    }

    IEnumerator AttackCooldownTimer(float seconds)
    {
        attackCooldown = true;
        yield return new WaitForSeconds(seconds);
        attackCooldown = false;
    }

    IEnumerator BufferJump(float seconds)
    {
        bufferingJump = true;
        yield return new WaitForSeconds(seconds);
        bufferingJump = false;
    }

    IEnumerator DashTimer(float seconds)
    {
        dashing = true;
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.color = Color.gray;
        yield return new WaitForSeconds(seconds);
        rend.color = Color.white;
        dashing = false;
    }

    IEnumerator FrostTimer(float frostTime, float frostCooldown)
    {
        canSpawnFrost = false;
        FrostAOE frost = Instantiate(frostAOEprefab, transform.position, transform.rotation);
        frost.owner = gameObject;
        yield return new WaitForSeconds(frostTime);
        Destroy(frost.gameObject);
        yield return new WaitForSeconds(frostCooldown);
        canSpawnFrost = true;
    }

    IEnumerator SlowDown(float slowTime)
    {
        slowed = true;
        yield return new WaitForSeconds(slowTime);
        slowed = false;
    }

    public bool canDoDash()
    {
        return canDash;
    }

    public bool canDoFrost()
    {
        return canSpawnFrost;
    }
}
