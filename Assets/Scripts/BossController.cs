using UnityEngine;

public class BossController : MonoBehaviour, IDamageable
{
    public int maxHealth = 1000;
    public int currentHealth;
    private BossHealthUI bossHealthUI;

    public void Init(BossHealthUI ui)
    {
        bossHealthUI = ui;
        currentHealth = maxHealth;

        bossHealthUI.Show();
        bossHealthUI.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (bossHealthUI != null)
            bossHealthUI.SetHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (bossHealthUI != null)
            bossHealthUI.Hide();

        Destroy(gameObject);
    }
}
