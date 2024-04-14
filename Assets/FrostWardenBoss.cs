using System;
using System.Collections;
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

    enum ActionTypes { Move, Jump, Attack };
    ActionTypes chosenAction;

    [Header("Frost")]
    [SerializeField] private FrostAOE frostAOEprefab;
    [SerializeField] private float frostTime = 2f;
    [SerializeField] private float frostCooldown = 4f;
    [SerializeField] private float slowDownTime = 2f;

    private GameManager gameManager;

    private Rigidbody2D myRigidBody;
    private Collider2D myCollider;

    bool grounded = true;
    bool attackCooldown = false;
    bool canSpawnFrost = true;
    bool slowed = false;
    bool canAttack = true;
    bool bufferingJump = false;

    protected override void Start()
    {
        base.Start();

        myRigidBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (canAttack)
            Action();

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
            case ActionTypes.Attack:
                StartSwing();
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

        StartCoroutine(AttackCooldown(walkTime));
    }

    private void StartJump()
    {
        Debug.Log("Jump");

        bufferingJump = true;

        StartMove();
    }

    public void Jump()
    {
        bufferingJump = false;
        // come back to this if you decide to give player acceleration

        myRigidBody.velocity = (slowed ? jumpVelocity / 1.4f : jumpVelocity) * Vector2.up;
    }

    private void StartSwing()
    {
        Debug.Log("Swing");

        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown(float bonusCooldownTime = 0)
    {
        yield return new WaitForSeconds(bonusCooldownTime + coolDownTime);
        walkDirection = Vector2.zero;
        canAttack = true;
    }
}
