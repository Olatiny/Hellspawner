using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class demonBossAI : MonoBehaviour
{
    bool canAttack = true;
    bool isSwooping = false;
    bool isScorchingRay = false;


    bool swoopLeftToRight = true;
    float attackCooldown = 3.0f;
    float swoopCooldown = 1.5f;

    float scorchingRayDuration = 2.25f;

    float bossFacingBeamOffset = 4.75f;
    float movementSpeed = 15.0f;
    float bossHealth = 100f;

    // Transforms to act as start and end markers for the journey.
    public Transform point1;
    public Transform point2;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    public int numberOfPossibleManuevers = 2;

    // Reference to the fire ray prefab
    public GameObject fireBeamPrefab;

    public GameObject player;

    private Rigidbody2D rb;


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
    }

    // private IEnumerator FireBeamDuration()
    // {
    //     // Wait for cooldown second
    //     yield return new WaitForSeconds(scorchingRayCooldown);

    //     isScorchingRay = false;
    // }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {

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
            // Distance moved equals elapsed time multiplied by move speed
            float distCovered = (Time.time - startTime) * movementSpeed;
            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;
            // Set our position as a fraction of the distance between the markers.
            //either swooping left to right
            if (swoopLeftToRight == true)
            {
                transform.position = Vector3.Lerp(point1.position, point2.position, fractionOfJourney);
            }
            else //or right to left
            {
                transform.position = Vector3.Lerp(point2.position, point1.position, fractionOfJourney);
            }


        }
        //scorching beam lazer thing
        else if (isScorchingRay)
        {
            //ray instantiated by scorching ray attack method
        }
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
                //swoopingAttack();

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
        //need to move to starting position
        //randomly select either side of map

        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(point1.position, point2.position);
        isSwooping = true;
        //reset swoop once done
        StartCoroutine(SwoopDash());

    }

    void scorchingRayAttack()
    {
        //face player?
        //transform.right = player.transform.position - transform.position;   
        //instantiate beam object pointing towards player
        //transform.LookAt(player.transform);
        //will be positive is player is to right of boss
        float turnDirection = player.transform.position.x - transform.position.x;
        //Debug.Log(player.transform.position);
        Vector3 turningOffset = new Vector3(bossFacingBeamOffset, 0.0f, 0.0f);
        // if (turnDirection <= 0)
        // {
        //     //shoot left
        //     //instantiate flame beam to the left
        //     var beam = Instantiate(fireBeamPrefab, (transform.position - turningOffset), Quaternion.identity);
        //     Vector3 directionToPlayer = player.transform.position - transform.position;
        //     beam.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        //     //beam.transform.rotation = Quaternion.RotateTowards(transform.rotation, player.transform.rotation, 360);
        //     //beam.transform.rotation.SetFromToRotation(transform.position, player.transform.position);
        //     //beam.transform.right = player.transform.position - beam.transform.position;
        //     //destroy beam after duration
        //     Destroy(beam, scorchingRayDuration);
        // }
        // else if (turnDirection > 0)
        // {
        //     //shoot right
        //     //instantiate flame beam to the left
        //     var beam = Instantiate(fireBeamPrefab, (transform.position + turningOffset), Quaternion.identity);
        //     //beam.transform.rotation = Quaternion.RotateTowards(transform.rotation, player.transform.rotation, 180);
        //     //destroy beam after duration
        //     Destroy(beam, scorchingRayDuration);
        // }
        var beam = Instantiate(fireBeamPrefab, (transform.position - turningOffset), Quaternion.identity);
        //Vector3 directionToPlayer = player.transform.position - transform.position;
        //beam.transform.rotation = 
        Quaternion.RotateTowards(transform.rotation, player.transform.rotation, 360);
        //beam.transform.rotation = Quaternion.LookRotation(directionToPlayer);
        //beam.transform.LookAt(player.transform);
        //Euler.angle

        //Vector3 direction = player.transform.position - beam.transform.position;

        // Set the local right direction of this object to point towards the target
        //beam.transform.right = direction.normalized;


        // Rotate the forward vector towards the target direction by one step
        //Vector3 targetDirection = player.transform.position - transform.position;
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 1.0f, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        //beam.transform.rotation = Quaternion.LookRotation(newDirection);
        //beam.transform.LookAt(player.transform.position);
        Destroy(beam, scorchingRayDuration);



    }

}
