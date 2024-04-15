using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichBoss : Boss
{

    public GameObject player;

    public List<Transform> teleportPoints;

    private Transform lastPosition = null;

    bool canAttack = true;

    [SerializeField] private float slowDownTime = 2f;
    bool slowed = false;

    [SerializeField]
    private float attackCooldown = 1.5f;

    [SerializeField]
    private float lichProjSpeedRegular = 8.0f;

    [SerializeField]
    private float lichProjSpeedSlowed = 7.5f;

    private Rigidbody2D rb;

    [SerializeField]
    private float lichSlowTeleDelay = .5f;

    [SerializeField]
    private LichBossProjectile lichprojectileprefab; //projectile prefab ref

    float attackDamage = 1f;

    int lastIndexOfTeleport;

    
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
        lastIndexOfTeleport = teleportIndex;
        //Debug.Log(teleportIndex);
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
            //save current transform to lastposition
            lastPosition = this.transform;
        }
        while (lastPosition == newPosition){
            newPosition = teleportPoints[Random.Range(0, teleportPoints.Count)];
        }
        //last spot isnt new spot so teleporta and save old spot
        //update current position to new position
        this.transform.position = newPosition.position;
        //save current transform to lastposition
        lastPosition = this.transform;
    }


    private IEnumerator ResetBoolAfterDelay()
    {
        if (slowed){
            yield return new WaitForSeconds(lichSlowTeleDelay);
        }
        // Wait for cooldown second
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void skullProjectileAttack()
    {
        LichBossProjectile lichprojectile = Instantiate(lichprojectileprefab, transform.position, transform.rotation);
        lichprojectile.Fire(player.GetComponent<PlayerController>(), attackDamage, (slowed ? lichProjSpeedSlowed : lichProjSpeedRegular));
        //lichprojectile.Fire(player.GetComponent<PlayerController>(), attackDamage, lichProjSpeedRegular);
    }

    protected override void OnDeath()
    {
        GameManager.Instance.beatLich();

        base.OnDeath();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FrostAOE")){
            StartCoroutine(SlowDown(slowDownTime));
        }

        base.OnTriggerEnter2D(collision);
    }

    IEnumerator SlowDown(float slowTime)
    {
        slowed = true;
        Debug.Log("lich: slowed");
        yield return new WaitForSeconds(slowTime);
        Debug.Log("lich: unslowed");
        slowed = false;
    }

}
