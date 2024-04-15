using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenBossProjectile : MonoBehaviour
{

    public float damageToDeal = 1f;

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
        FrostAOE faoe = Instantiate<FrostAOE>(new(), transform.position, transform.rotation);
        FindAnyObjectByType<FrostWardenBoss>()?.IcicleCloud(faoe);

        Debug.Log("LichSkullHitSum");
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.PlayerTakeDamage((int)damageToDeal);
            //any effect of hit?
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            //player attack and icicle collide, destroy icicle
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
