using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<EnemyWave> waves;
    public float delayBetweenWaves = 2f;

    private int currentWaveIndex = 0;

    public GameObject bossPref;
    public Transform bossSpawnPoint;
    private bool bossSpawned = false;

    [SerializeField] private BossHealthUI bossHealthUI;

    private void Start()
    {
        StartCoroutine(HandleWaves());
    }

    IEnumerator HandleWaves()
    {
        SpawnWave(currentWaveIndex);
        currentWaveIndex++;

        while (currentWaveIndex < waves.Count)
        {
            yield return new WaitUntil(() => IsWaveCleared(currentWaveIndex - 1));
            yield return new WaitForSeconds(delayBetweenWaves);

            SpawnWave(currentWaveIndex);
            currentWaveIndex++;
        }

        yield return new WaitUntil(() => IsWaveCleared(currentWaveIndex - 1));
        yield return new WaitForSeconds(0.5f);

        if (!bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true;
        }
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

    bool IsWaveCleared(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waves.Count) return true;

        EnemyWave wave = waves[waveIndex];
        wave.spawnedEnemies.RemoveAll(e => e == null);

        return wave.spawnedEnemies.Count == 0;
    }

    void SpawnBoss()
    {
        if (bossPref == null || bossSpawnPoint == null || bossHealthUI == null)
        {
            Debug.LogError("❌ Не хватает ссылок для спавна босса!");
            return;
        }

        GameObject bossGO = Instantiate(bossPref, bossSpawnPoint.position, Quaternion.identity);

        BossController bossController = bossGO.GetComponent<BossController>();
        if (bossController != null)
        {
            bossController.Init(bossHealthUI);
        }
        else
        {
            Debug.LogError("❌ У босса отсутствует скрипт BossController!");
        }

        bossSpawned = true;
}

}
