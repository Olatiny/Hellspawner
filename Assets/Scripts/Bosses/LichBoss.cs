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

    Animator myAnimator;

    [SerializeField]
    private ParticleSystem particles;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        myAnimator = GetComponent<Animator>();

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

    void teleport(Transform lastPosition, Transform newPosition)
    {
        AudioManager.Instance?.LichTeleportSFX();

        particles.Stop();
        particles.Play();

        Debug.Log("teleporting lich boss");
        //lastpos will be null if lich hasn't teleported yet
        if (lastPosition == null)
        {
            //update current position to new position
            this.transform.position = newPosition.position;
            //save current transform to lastposition
            lastPosition = this.transform;
        }
        while (lastPosition == newPosition)
        {
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
        if (slowed)
        {
            yield return new WaitForSeconds(lichSlowTeleDelay);
        }

        myAnimator.SetBool("Attacking", true);
        int numExtraSkulls = Random.Range(0, 3);
        for (int i = 0; i < numExtraSkulls; i++)
        {
            skullProjectileAttack();
            yield return new WaitForSeconds(attackCooldown * (i + 1) / numExtraSkulls);
        }

        // Wait for cooldown second
        myAnimator.SetBool("Attacking", false);
        yield return new WaitForSeconds(attackCooldown * .4f);
        canAttack = true;
    }

    void skullProjectileAttack()
    {
        AudioManager.Instance?.SkullShootSFX();

        LichBossProjectile lichprojectile = Instantiate(lichprojectileprefab, transform.position, transform.rotation);
        lichprojectile.Fire(player.GetComponent<PlayerController>(), attackDamage, (slowed ? lichProjSpeedSlowed : lichProjSpeedRegular));
        //lichprojectile.Fire(player.GetComponent<PlayerController>(), attackDamage, lichProjSpeedRegular);
    }

    protected override void OnDeath()
    {
        GameManager.Instance.beatLich();

        base.OnDeath();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FrostAOE"))
        {
            StartCoroutine(SlowDown(slowDownTime));
        }

        base.OnTriggerEnter2D(collision);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("FrostAOE") && gameObject.GetComponent<FrostAOE>().owner != gameObject)
            StartCoroutine(SlowDown(slowDownTime));
    }

    IEnumerator SlowDown(float slowTime)
    {
        slowed = true;
        GetComponent<SpriteRenderer>().color = Color.cyan;
        Debug.Log("lich: slowed");
        yield return new WaitForSeconds(slowTime);
        GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log("lich: unslowed");
        slowed = false;
    }

}
