using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    public int damage = 1; // Можно настраивать урон пули в инспекторе

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

