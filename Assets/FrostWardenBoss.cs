using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostWardenBoss : Boss
{
    [Header("Platforming")]
    [SerializeField] private float gravity = 75f;
    [SerializeField] private float jumpVelocity = 15f;
    [SerializeField] private float jumpHoldTime = .15f;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float jumpBufferTime = .1f;
    [SerializeField] private float jumpRaycastDist = .8f;
    [SerializeField] private float walktimeMin;
    [SerializeField] private float walktimeMax;

    private float walkTime;
    private Vector2 walkDirection;

    [Header("Attack Stuff")]
    [SerializeField] float coolDownTime = .5f;
    [SerializeField] float cloudSeconds = 2f;

    enum ActionTypes { Move, Jump };
    ActionTypes chosenAction;

    [Header("Frost")]
    [SerializeField] private FrostAOE frostAOEprefab;
    [SerializeField] private float frostTime = 2f;
    [SerializeField] private float frostCooldown = 4f;
    [SerializeField] private float slowDownTime = 2f;
    [SerializeField] List<Transform> icicleYSpawnLevels;
    [SerializeField] private float icicleRandomSpread = 1.5f;

    private GameManager gameManager;

    private Rigidbody2D myRigidBody;
    private Collider2D myCollider;
    private Animator myAnimator;

    public WardenBossProjectile iciclePrefab;

    bool grounded = true;
    bool attackCooldown = false;
    bool canSpawnFrost = true;
    bool slowed = false;
    bool canAttack = true;
    bool bufferingJump = false;
    bool moving = false;
    bool jumping = false;

    protected override void Start()
    {
        base.Start();

        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canAttack)
            Action();

        if (walkDirection.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
            GetComponent<SpriteRenderer>().flipX = false;

        // Gravity if not jumping
        if (myRigidBody.velocity.y > -maxFallSpeed)
            myRigidBody.velocity += (slowed ? gravity / 2 : gravity) * Time.fixedDeltaTime * Vector2.down;

        // Check if Grounded 
        RaycastHit2D hit = Physics2D.CircleCast(gameObject.transform.position, .5f, Vector2.down, jumpRaycastDist);
        //(gameObject.transform.position, Vector2.down * 1.5f, Color.black, .05f);
        grounded = hit.collider != null && hit.collider != myCollider;

        // Buffer Jump
        if (bufferingJump)
            Jump();

        myAnimator.SetBool("Walking", moving);

        if (!moving)
            return;

        // Movement
        if (walkDirection.x != 0)
        {
            myRigidBody.velocity = new(walkDirection.x * walkSpeed, myRigidBody.velocity.y);
        }
        else
            myRigidBody.velocity = new(0, myRigidBody.velocity.y);

        if (slowed)
            myRigidBody.velocity = new(myRigidBody.velocity.x / 2, myRigidBody.velocity.y);
    }

    protected override void OnDeath()
    {
        GameManager.Instance.beatfrostWarden();

        base.OnDeath();
    }

    private void Action()
    {
        canAttack = false;

        chosenAction = (ActionTypes)UnityEngine.Random.Range(0, Enum.GetNames(typeof(ActionTypes)).Length);

        switch (chosenAction)
        {
            case ActionTypes.Move:
                StartMove();
                break;
            case ActionTypes.Jump:
                StartJump();
                break;
            default:
                break;
        }
    }

    private void StartMove()
    {
        Debug.Log("Move");

        int dir = (FindAnyObjectByType<PlayerController>().transform.position - transform.position).x < 0 ? -1 : 1;

        walkDirection = new(dir, 0);
        walkTime = UnityEngine.Random.Range(walktimeMin, walktimeMax + 1);

        moving = true;

        StartCoroutine(AttackCooldown(walkTime));
    }

    private void StartJump()
    {
        Debug.Log("Jump");

        bufferingJump = true;

        myAnimator.SetBool("Jumping", true);
        StartMove();
    }

    public void Jump()
    {
        AudioManager.Instance?.TrollJumpSFX();

        bufferingJump = false;
        // come back to this if you decide to give player acceleration

        myRigidBody.velocity = (slowed ? jumpVelocity / 1.4f : jumpVelocity) * Vector2.up;

        StartCoroutine(JumpingBoolCooldown());
    }

    private void StartSwing()
    {
        Debug.Log("Swing");

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown(float bonusCooldownTime = 0)
    {
        yield return new WaitForSeconds(bonusCooldownTime);
        //walkDirection = Vector2.zero;
        moving = false;
        myRigidBody.velocity = new(0, myRigidBody.velocity.y);

        yield return new WaitForSeconds(coolDownTime);
        canAttack = true;
    }

    IEnumerator JumpingBoolCooldown()
    {
        //let frost warden leave ground then set jumping to true for ground return/icicle summon
        yield return new WaitForSeconds(.5f);
        jumping = true; //set jumping to true for icicle
    }

    //protected override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    base.OnTriggerEnter2D(collision);
    //}

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && jumping)
        {
            //check for ground hit once jumping
            AudioManager.Instance?.TrollLandingSFX();
            jumping = false;
            myAnimator.SetBool("Jumping", false);
            createIcicle();
        }
        base.OnCollisionEnter2D(collision);
    }

    private void createIcicle()
    {

        Debug.Log("icicleTime");
        foreach (Transform icicleYSpawnLevel in icicleYSpawnLevels)
        {
            WardenBossProjectile icicleFalling = Instantiate(iciclePrefab, icicleYSpawnLevel.position, transform.rotation);
        }
    }

    public void IcicleCloud(FrostAOE aoe)
    {
        aoe.owner = gameObject;

        StartCoroutine(KillCloud(aoe));
    }

    IEnumerator KillCloud(FrostAOE aoe)
    {
        yield return new WaitForSeconds(cloudSeconds);
        Destroy(aoe.gameObject);
    }
}
