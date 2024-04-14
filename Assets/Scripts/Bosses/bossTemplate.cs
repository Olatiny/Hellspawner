using UnityEngine;

public class bossTemplate : MonoBehaviour
{
    public float bossMaxHealth = 100f;
    public float bossCurrentHealth;

    protected virtual void Start()
    {
        bossCurrentHealth = bossMaxHealth;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage Player
        }
    }

    public void TakeDamage(float damage)
    {
        bossCurrentHealth -= damage;

        if (bossCurrentHealth <= 0)
            Die();
    }

    private void Die()
    {
        // TODO: Let Game Manager know death occurred.
    }
}

