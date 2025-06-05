using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public float spawnTime;
    public bool useAutoLayout = false;

    public float layoutWidth = 6f;
    public float layoutHeight = 4f;
    public int rows = 1;
    public int columns = 5;

    public List<GameObject> autoLayoutPrefs;

    [HideInInspector] public List<GameObject> spawnedEnemies = new();
}

