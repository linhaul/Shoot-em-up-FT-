using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<EnemyWave> waves = new List<EnemyWave>();
    private float timer;
    private int waveIndex = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        while (waveIndex < waves.Count && timer >= waves[waveIndex].spawnTime)
        {
            SpawnWave(waves[waveIndex]);
            waveIndex++;
        }
    }

    private void SpawnWave(EnemyWave wave)
{
    if (wave.useAutoLayout)
    {
        SpawnAutoLayoutWave(wave);
    }
    else
    {
        foreach (var enemyInfo in wave.enemies)
        {
            SpawnEnemy(enemyInfo.pref, enemyInfo.position, enemyInfo.movementPattern, enemyInfo.firePattern);
        }
    }
}

private void SpawnAutoLayoutWave(EnemyWave wave)
{
    float startX = -wave.layoutWidth / 2f;
    float startY = 6f; // начальная y позиция (выше экрана)
    float stepX = wave.layoutWidth / Mathf.Max(1, wave.columns - 1);
    float stepY = wave.layoutHeight / Mathf.Max(1, wave.rows - 1);

    int prefabIndex = 0;

    for (int row = 0; row < wave.rows; row++)
    {
        for (int col = 0; col < wave.columns; col++)
        {
            if (prefabIndex >= wave.autoLayoutPrefs.Count)
                return;

            GameObject prefab = wave.autoLayoutPrefs[prefabIndex];
            Vector2 spawnPos = new Vector2(startX + col * stepX, startY + row * -stepY);
            SpawnEnemy(prefab, spawnPos, "StraightDown", "AimAtPlayer");

            prefabIndex++;
        }
    }
}


private void SpawnEnemy(GameObject prefab, Vector2 position, string movePattern, string firePattern)
{
    GameObject enemy = Instantiate(prefab, position, Quaternion.identity);

    var shooter = enemy.GetComponent<EnemyShooter>();
    if (shooter != null && System.Enum.TryParse(firePattern, out EnemyShooter.FirePattern fire))
    {
        shooter.firePattern = fire;
    }

    var mover = enemy.GetComponent<EnemyMovement>();
    if (mover != null && System.Enum.TryParse(movePattern, out EnemyMovement.MovementType movement))
    {
        mover.movementType = movement;
    }
}

}
