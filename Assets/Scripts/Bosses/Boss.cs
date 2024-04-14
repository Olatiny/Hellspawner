using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
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
            GameManager.Instance.PlayerTakeDamage(1);
    }

    public void TakeDamage(float damage)
    {
        bossCurrentHealth -= damage;

        if (bossCurrentHealth <= 0)
            OnDeath();
    }

    protected virtual void OnDeath()
    {
        SceneManager.LoadScene("BossSelect");
    }
}

