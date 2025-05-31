using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public int damageToEnemy = 25;
    public int damageToPlayer = 1;
    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && playerHealth != null)
        {
            if (!playerHealth.IsInvulnerable())
            {
                playerHealth.TakeDamage(damageToPlayer);

                Enemy enemyHealth = collision.GetComponent<Enemy>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damageToEnemy);
                }
            }
        }
    }
}
