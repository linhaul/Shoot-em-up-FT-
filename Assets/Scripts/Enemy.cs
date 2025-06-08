using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int Health = 3;

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
