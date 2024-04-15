using UnityEngine;

public class WardenBossProjectile : MonoBehaviour
{

    public float damageToDeal = 1f;

    [SerializeField]
    private FrostAOE aoePrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance?.IcicleImpactSFX();

        Debug.Log("LichSkullHitSum");
        if (collision.gameObject.CompareTag("Player"))
        {
            FindAnyObjectByType<PlayerController>().TakeDamage((int)damageToDeal);
            //any effect of hit?
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            //player attack and icicle collide, destroy icicle
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            FrostAOE faoe = Instantiate(aoePrefab, transform.position, transform.rotation);
            FindAnyObjectByType<FrostWardenBoss>()?.IcicleCloud(faoe);

            Destroy(gameObject);
        }
    }
}
