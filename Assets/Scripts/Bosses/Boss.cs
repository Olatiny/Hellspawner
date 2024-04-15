using UnityEngine;

public class Boss : MonoBehaviour
{
    public float bossMaxHealth = 100f;
    public float bossCurrentHealth;

    protected virtual void Start()
    {
        bossCurrentHealth = bossMaxHealth;
        GameManager.Instance.UpdateBossHealthBar(this);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            FindAnyObjectByType<PlayerController>().TakeDamage(1);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            FindAnyObjectByType<PlayerController>().TakeDamage(1);
    }

    public void TakeDamage(float damage)
    {
        bossCurrentHealth -= (damage);

        GameManager.Instance.UpdateBossHealthBar(this);

        if (bossCurrentHealth <= 0)
            OnDeath();
    }

    protected virtual void OnDeath()
    {
        GameManager.Instance.Victory();
    }
}

