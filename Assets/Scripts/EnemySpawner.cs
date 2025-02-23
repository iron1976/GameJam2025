using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnConfig
    {
        public GameObject enemyPrefab;
        public float spawnRate;
        
        public int maxEnemies;
    }

    [Header("Spawn Settings")]
    public SpawnConfig[] daySpawnConfigs;
    public SpawnConfig[] nightSpawnConfigs;
    public Transform[] spawnPoints;
    
    [Header("Time Settings")]
    public float nightTimeSpawnMultiplier = 2f;
    public bool isNightTime;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnConfig[] currentConfigs = isNightTime ? nightSpawnConfigs : daySpawnConfigs;
            
            foreach (SpawnConfig config in currentConfigs)
            {
                if (ShouldSpawn(config))
                {
                    SpawnEnemy(config);
                }
            }

            float waitTime = isNightTime ? 
                Random.Range(0.5f, 1.5f) / nightTimeSpawnMultiplier : 
                Random.Range(1f, 3f);
                
            yield return new WaitForSeconds(waitTime);
        }
    }

    private bool ShouldSpawn(SpawnConfig config)
    {
        int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        return currentEnemyCount < config.maxEnemies && Random.value < config.spawnRate;
    }

    private void SpawnEnemy(SpawnConfig config)
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(config.enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    // Call this method when the day/night cycle changes
    public void SetNightTime(bool isNight)
    {
        isNightTime = isNight;
    }
}
    
