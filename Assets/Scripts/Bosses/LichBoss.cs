using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichBoss : Boss
{

    public GameObject player;

    public List<Transform> teleportPoints;

    private Transform lastPosition = null;

    bool canAttack = true;

    float attackCooldown = 1.5f;

    private Rigidbody2D rb;

    [SerializeField]
    private LichBossProjectile lichprojectileprefab; //projectile prefab ref

    float attackDamage = 1f;

    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            Debug.Log("new Attack from lich boss");
            canAttack = false;
            attack();
            // Start the coroutine to reset attack flag
            StartCoroutine(ResetBoolAfterDelay());
        }
    }

    //lich teleports then attacks each attack
    void attack()
    {
        //select random spot from teleport points
        int teleportIndex = Random.Range(0, teleportPoints.Count);
        Debug.Log(teleportIndex);
        //teleport to new spot if new
        teleport(lastPosition, teleportPoints[teleportIndex]);
        //and shoot projectile at player
        skullProjectileAttack();
    }

    void teleport(Transform lastPosition, Transform newPosition){
        Debug.Log("teleporting lich boss");
        //lastpos will be null if lich hasn't teleported yet
        if (lastPosition == null){
            //update current position to new position
            this.transform.position = newPosition.position;
        }
        else if (lastPosition == newPosition){
            //recall teleport with new random
        }
        else{
            //last spot isnt new spot
            //save current transform to lastposition
            lastPosition = this.transform;
            //update current position to new position
            teleport(lastPosition, teleportPoints[Random.Range(0, teleportPoints.Count)]);
        }
    }


    private IEnumerator ResetBoolAfterDelay()
    {
        // Wait for cooldown second
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    void skullProjectileAttack()
    {
        LichBossProjectile lichprojectile = Instantiate(lichprojectileprefab, transform.position, transform.rotation);
        lichprojectile.Fire(player.GetComponent<PlayerController>(), attackDamage);
    }
}
