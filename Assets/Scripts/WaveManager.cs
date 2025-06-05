using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<EnemyWave> waves;
    public float delayBetweenWaves = 2f;

    private int currentWaveIndex = 0;
    private float timer = 0f;
    private bool waitingForClear = false;

    public GameObject bossPref;
    public Transform bossSpawnPoint;
    private bool bossSpawned = false;

    public BossHealthUI bossHealthUI;

    void Update()
    {
        if (currentWaveIndex >= waves.Count)
            return;

        if (currentWaveIndex == 0)
        {
            timer += Time.deltaTime;
            if (timer >= waves[0].spawnTime)
            {
                SpawnWave(currentWaveIndex);
                currentWaveIndex++;
                waitingForClear = true;
            }
        }
        else
        {
            if (waitingForClear)
            {
                if (IsWaveCleared(currentWaveIndex - 1))
                {
                    StartCoroutine(SpawnNextWaveWithDelay());
                    waitingForClear = false;
                }
            }
        }

        if (currentWaveIndex >= waves.Count && !bossSpawned)
        {
            if (AllEnemiesDefeated())
            {
                SpawnBoss();
                bossSpawned = true;
            }
        }
    }

    bool AllEnemiesDefeated()
    {
        foreach (var wave in waves)
        {
            wave.spawnedEnemies.RemoveAll(e => e == null);
            if (wave.spawnedEnemies.Count > 0)
            {
                return false;
            }
        }
        return true;
    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPref, bossSpawnPoint.position, Quaternion.identity);

        BossController bossController = boss.GetComponent<BossController>();
        if (bossController != null && bossHealthUI != null)
        {
            bossController.SetBossHealthUI(bossHealthUI);
        }
    }

    IEnumerator SpawnNextWaveWithDelay()
    {
        yield return new WaitForSeconds(delayBetweenWaves);

        if (currentWaveIndex < waves.Count)
        {
            SpawnWave(currentWaveIndex);
            currentWaveIndex++;
            waitingForClear = true;
        }
    }

    bool IsWaveCleared(int waveIndex)
    {
        var wave = waves[waveIndex];
        wave.spawnedEnemies.RemoveAll(e => e == null);

        return wave.spawnedEnemies.Count == 0;
    }

    void SpawnWave(int index)
    {
        EnemyWave wave = waves[index];
        wave.spawnedEnemies = new();

        if (wave.useAutoLayout)
        {
            float startX = -wave.layoutWidth / 2f;
            float startY = wave.layoutHeight / 2f;

            int prefabIndex = 0;

            for (int row = 0; row < wave.rows; row++)
            {
                for (int col = 0; col < wave.columns; col++)
                {
                    if (prefabIndex >= wave.autoLayoutPrefs.Count) return;

                    GameObject prefab = wave.autoLayoutPrefs[prefabIndex];
                    Vector2 spawnPos = new Vector2(
                        startX + col * (wave.layoutWidth / Mathf.Max(1, wave.columns - 1)),
                        startY - row * (wave.layoutHeight / Mathf.Max(1, wave.rows - 1))
                    );

                    GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
                    wave.spawnedEnemies.Add(enemy);

                    prefabIndex++;
                }
            }
        }
    }
}
