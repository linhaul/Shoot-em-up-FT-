using UnityEngine;

public class BombPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerBombSystem bombSystem = collision.GetComponent<PlayerBombSystem>();
            if (bombSystem != null)
            {
                bombSystem.AddBomb();
                Destroy(gameObject);
            }
        }
    }
}

