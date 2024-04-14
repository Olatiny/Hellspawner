using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float speed = 1f;

    private PlayerController player;

    private Vector2 direction;

    bool flying = false;

    float damage = 1f;

    private void FixedUpdate()
    {
        if (flying)
            transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore collisions with self
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("PlayerAttack") || collision.gameObject.CompareTag("Laser"))
            return;

        // Damage Enemy
        if (collision.gameObject.CompareTag("Enemy"))
            collision.gameObject.GetComponent<Boss>().TakeDamage(damage);

        //if (collision.gameObject.CompareTag("EnemyAttack"))
        //    print("Projectile hit with player projectile, destroy it when implemented"); // TODO: <- what he said.

        // We hit a wall
        KillProjectile();
    }

    public void SendProjectile(PlayerController player, float speed, Vector2 direction, float damage)
    {
        this.player = player;
        this.direction = direction;
        this.speed = speed;
        this.damage = damage;

        flying = true;
    }

    public void KillProjectile()
    {
        if (player.projectiles.Contains(this) && flying)
        {
            player.projectiles.Remove(this);
            Destroy(gameObject);
        }
    }
}
