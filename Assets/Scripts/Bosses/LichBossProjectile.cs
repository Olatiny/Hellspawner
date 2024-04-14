using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichBossProjectile : MonoBehaviour
{
    public float activeSeconds = .2f;
    bool lethal = true;
    bool rotating = false;
    public PlayerController player;
    public float damageToDeal = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    private void Update()
    {
        if (!rotating)
            return;

        Vector2 dir = (player.transform.position - transform.position).normalized;
        float angle = -1 * Vector2.SignedAngle(dir, Vector2.right);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && lethal)
        {
            GameManager.Instance.PlayerTakeDamage((int)damageToDeal);
            lethal = false;
            //any effect of skull hit?
            Destroy(gameObject);
        }
    }

    public void Fire(PlayerController player, float damage)
    {
        this.player = player;
        damageToDeal = damage;
        //rotate towards player and shoot straight (not homing)
        StartCoroutine(FireSkull());
        
    }

    IEnumerator FireSkull()
    {
        rotating = true;

        //yield return new WaitForSeconds(chargeSeconds * .67f);
        //transform.GetComponentInChildren<SpriteRenderer>().color = Color.red * new Color(.6f, .6f, .6f);
        // transform.localScale = new(1, 1, 1);
        //lethal = false;

        yield return new WaitForSeconds(activeSeconds);

        

        
    }
}
