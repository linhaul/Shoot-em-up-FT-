using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    public int damage = 1;
    public float superMeterPerHit = 1.5f;
    public GameObject hitEffectPrefab; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);

                if (PlayerShooting.Instance != null)
                {
                    PlayerShooting.Instance.AddSuperMeter(superMeterPerHit);
                }
            }

            Destroy(gameObject);
        }
    }
}
