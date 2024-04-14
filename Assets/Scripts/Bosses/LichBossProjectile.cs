using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichBossProjectile : MonoBehaviour
{
    public float activeSeconds = .2f;
    bool lethal = true;
    bool flying = false;
    bool rotating = false;
    public PlayerController player;
    public float damageToDeal = 1f;
    private float proj_Speed = 10.0f;
    private Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        //set initial direction to continue going towards until miss or hit
        direction = (player.transform.position - transform.position).normalized;
    }

    private void FixedUpdate()
    {
        if (flying){
            float step = proj_Speed * Time.fixedDeltaTime;
            transform.Translate(step * direction);
        }

        //send forward //update direction
        
        //transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + Vector2.up, step);

        

        
        //var heading = 2 * (target.position - transform.position);
        // move sprite towards the target location
        //transform.position = Vector2.MoveTowards(transform.position, target, step);

        
        // if (flying){

        // }
    }

    // Update is called once per frame
    private void Update()
    {

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("LichSkullHitSum");
        if (collision.gameObject.CompareTag("Player") && lethal)
        {
            GameManager.Instance.PlayerTakeDamage((int)damageToDeal);
            lethal = false;
            //any effect of skull hit?
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            Debug.Log("LichSkullPlayerAttackCOllsion");
            lethal = false;
            //player attack and skull collide, destroy skull
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

        yield return new WaitForSeconds(.25f);

        flying = true;
        //transform.GetComponentInChildren<SpriteRenderer>().color = Color.red * new Color(.6f, .6f, .6f);
        // transform.localScale = new(1, 1, 1);
        // //lethal = false;

        yield return new WaitForSeconds(activeSeconds);
        Destroy(gameObject);

        

        
    }
}