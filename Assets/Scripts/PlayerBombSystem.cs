using Unity.Mathematics;
using UnityEngine;

public class PlayerBombSystem : MonoBehaviour
{
    public int bombCount = 3;
    public int maxBombs = 3;
    public float bombDamage = 100f;
    public PlayerUIManager uIManager;

    private void Awake()
    {
        UpdateUI();
    }
    private void Update()
    {
        if (GameOverMenuController.IsGameOver)
            return;

        if (Input.GetKeyDown(CombatKeybindManager.BombKey) && bombCount != 0)
        {
            UseBomb();
        }
    }

    public void AddBomb()
    {
        if (bombCount < maxBombs)
        {
            bombCount++;
            UpdateUI();
        }
    }

    public void UseBomb()
    {
        bombCount--;
        UpdateUI();

        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject bullet in enemyBullets)
        {
            Destroy(bullet);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage((int)bombDamage);
            }
        }
    }

    void UpdateUI()
    {
        if (uIManager != null)
        {
            uIManager.UpdateBombCount(bombCount);
        }
    }
}
