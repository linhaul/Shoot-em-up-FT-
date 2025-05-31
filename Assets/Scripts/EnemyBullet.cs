using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null && !playerHealth.IsInvulnerable())
            {
                playerHealth.TakeDamage(damage);
                Destroy(gameObject); 
            }
        }
        
    }
}
