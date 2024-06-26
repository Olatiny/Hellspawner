using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public PlayerController player;

    public float chargeSeconds = .5f;
    public float activeSeconds = .2f;
    public float damageToDeal = 1f;

    bool lethal = false;
    bool rotating = false;
    bool slowed = false;

    private void Update()
    {
        if (!rotating)
            return;

        if (player != null)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized;
            float angle = -1 * Vector2.SignedAngle(dir, Vector2.right);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && lethal)
        {
            FindAnyObjectByType<PlayerController>().TakeDamage((int)damageToDeal);
            lethal = false;
        }
    }

    public void Fire(PlayerController player, float damage, bool slowed)
    {
        this.player = player;
        damageToDeal = damage;
        this.slowed = slowed;
        StartCoroutine(FireLaser(slowed));
    }

    IEnumerator FireLaser(bool slowed)
    {
        transform.GetComponentInChildren<SpriteRenderer>().color = new Color(.3f, .3f, .3f);
        transform.localScale = new(1, .5f, 1);
        rotating = true;

        yield return new WaitForSeconds(chargeSeconds * (slowed ? .67f * 2 : .67f));

        rotating = false;
        transform.GetComponentInChildren<SpriteRenderer>().color = new Color(.6f, .6f, .6f);

        yield return new WaitForSeconds(chargeSeconds * (slowed ? .33f * 2 : .33f));

        lethal = true;

        transform.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        transform.localScale = new(1, 1, 1);

        yield return new WaitForSeconds(activeSeconds);

        lethal = false;

        Destroy(gameObject);
    }
}
