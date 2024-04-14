using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public PlayerController player;

    public float chargeSeconds = .5f;
    public float activeSeconds = .2f;
    public float damageToDeal = 1f;

    bool rotating = false;

    private void Update()
    {
        if (!rotating)
            return;

        Vector2 dir = (player.transform.position - transform.position).normalized;
        float angle = -1 * Vector2.SignedAngle(dir, Vector2.right);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            print("Projectile hit player, damage it when implemented"); // TODO: <- what he said.
    }

    public void Fire(PlayerController player, float damage)
    {
        this.player = player;
        damageToDeal = damage;
        StartCoroutine(FireLaser());
    }

    IEnumerator FireLaser()
    {
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.red * new Color(.3f, .3f, .3f);
        transform.localScale = new(1, .5f, 1);
        rotating = true;

        yield return new WaitForSeconds(chargeSeconds * .67f);

        rotating = false;
        transform.GetComponentInChildren<SpriteRenderer>().color = Color.red * new Color(.6f, .6f, .6f);

        yield return new WaitForSeconds(chargeSeconds * .33f);

        transform.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        transform.localScale = new(1, 1, 1);

        yield return new WaitForSeconds(activeSeconds);

        Destroy(gameObject);
    }
}
