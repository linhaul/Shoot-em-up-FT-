using UnityEngine;

public class BossController : MonoBehaviour
{
    public int maxHealth = 1000;
    private int currentHealth;

    private BossHealthUI bossHealthUI;

    private void Start()
    {
        currentHealth = maxHealth;

        if (bossHealthUI != null)
        {
            bossHealthUI.gameObject.SetActive(true);
            bossHealthUI.SetMaxHealth(maxHealth);
        }
    }

    public void SetBossHealthUI(BossHealthUI ui)
    {
        bossHealthUI = ui;
        if (bossHealthUI != null)
        {
            bossHealthUI.gameObject.SetActive(true);
            bossHealthUI.SetMaxHealth(maxHealth);
            bossHealthUI.SetHealth(currentHealth);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (bossHealthUI != null)
        {
            bossHealthUI.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (bossHealthUI != null)
        {
            bossHealthUI.gameObject.SetActive(false);
        }

        Destroy(gameObject);
    }
}
