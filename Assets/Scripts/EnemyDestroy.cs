using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    public int damage = 1;
    public float superMeterPerHit = 1.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                if (PlayerShooting.Instance != null)
                {
                    PlayerShooting.Instance.AddSuperMeter(superMeterPerHit);
                }
            }

            Destroy(gameObject);
        }
    }
}

