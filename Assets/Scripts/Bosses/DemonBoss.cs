using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss : Boss
{
    bool canAttack = true;
    bool isSwooping = false;
    bool isScorchingRay = false;

    bool swoopLeftToRight = true;
    float attackCooldown = 1.5f;
    float swoopCooldown = 1.5f;

    float scorchingRayDuration = 2.25f;
    float attackDamage = 1f;

    float bossFacingBeamOffset = 4.75f;
    public float movementSpeed = 30f;
    float bossHealth = 100f;

    // Transforms to act as start and end markers for the journey.
    public List<Transform> movePoints;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public int numberOfPossibleManuevers = 2;

    // Reference to the fire ray prefab
    public Laser laserPrefab;

    public GameObject player;

    private Rigidbody2D rb;

    Transform destination = null;
    Transform source = null;

    Animator myAnimator;

    Vector2 dir;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        player = FindAnyObjectByType<PlayerController>().gameObject;

        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            dir = player.transform.position - transform.position;

            if (dir.x < 0)
                GetComponent<SpriteRenderer>().flipX = true;
            else
                GetComponent<SpriteRenderer>().flipX = false;
        }

        //Debug.Log(isSwooping);
        if (bossHealth <= 0)
        {
            //player wins fight
            //player gets new ability
        }
        //starts new attacks when it can, set canAttack to true via coroutine + cooldown
        if (canAttack)
        {
            Debug.Log("new Attack from demon boss");
            canAttack = false;
            attack();
            // Start the coroutine to reset attack flag
            StartCoroutine(ResetBoolAfterDelay());
        }
        //dashing/swoop attack
        else if (isSwooping)
        {
            //StartCoroutine(SwoopDash());
            Debug.Log("swooping");

            float distCovered = (Time.time - startTime) * movementSpeed;

            float fractionOfJourney = distCovered / journeyLength;

            transform.position = Vector2.Lerp(source.position, destination.position, fractionOfJourney * Time.deltaTime);
        }
        //scorching beam lazer thing
        else if (isScorchingRay)
        {
            //ray instantiated by scorching ray attack method
        }
    }

    protected override void OnDeath()
    {
        GameManager.Instance.BeatDemon();

        base.OnDeath();
    }

    void attack()
    {
        //random attack chosen
        int attackChosen = Random.Range(1, numberOfPossibleManuevers + 1);
        //int attackChosen = 1;
        switch (attackChosen)
        {
            case 1:

                //execute attack
                //int leftOrRight = Random.Range(0, 2); //either 0 or 1
                //swooping either from left to right or right to left (at random)
                if (swoopLeftToRight == false)
                {
                    Debug.Log("demon: swoopAttackL2R");
                    swoopLeftToRight = true;
                }
                else
                {
                    Debug.Log("demon: swoopAttackR2L");
                    swoopLeftToRight = false;
                }
                swoopingAttack();

                break;

            case 2:
                Debug.Log("demon: ray attackChosen");
                //execute attack
                scorchingRayAttack();
                break;

            case 3:
                Debug.Log("boss: third attackChosen");
                //execute attack

                break;

            default:
                Debug.Log("boss: default");
                //shouldn't get here
                break;
        }

    }

    void swoopingAttack()
    {
        AudioManager.Instance?.HellChargeSFX();
        myAnimator.SetBool("Attacking", false);

        while (destination == null || destination.position.Equals(source.position))
        {
            destination = movePoints[Random.Range(0, movePoints.Count)];
            source = transform;
        }

        // Keep a note of the time the movement started.
        startTime = Time.time;

        journeyLength = Vector3.Distance(destination.position, source.position);

        isSwooping = true;
        StartCoroutine(SwoopDash());
    }

    void scorchingRayAttack()
    {
        AudioManager.Instance?.ScorchRaySFX();

        myAnimator.SetBool("Attacking", true);

        Laser beam = Instantiate(laserPrefab, transform.position + (dir.x < 0 ? new(-.5f, .5f) : new(.5f, .5f)), transform.rotation);
        beam.Fire(player.GetComponent<PlayerController>(), attackDamage);
    }

    private IEnumerator ResetBoolAfterDelay()
    {
        // Wait for cooldown second
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private IEnumerator SwoopDash()
    {
        // Wait for cooldown second
        yield return new WaitForSeconds(swoopCooldown);

        isSwooping = false;
        destination = null;
    }
}
