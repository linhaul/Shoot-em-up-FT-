using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPref;
    public Transform[] spawnPoints;
    public float spawninterval = 5f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawninterval)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || enemyPref == null) return;

        Transform point = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPref, point.position, Quaternion.identity);
    }
}
