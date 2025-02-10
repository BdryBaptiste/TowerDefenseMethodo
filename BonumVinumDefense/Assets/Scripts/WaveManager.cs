using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName; // Name of the wave
        public List<EnemySpawnInfo> enemies; // List of enemies to spawn
    }

    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab; // Enemy type to spawn
        public int count; // Number of enemies
        public float spawnDelay; // Delay between spawns
    }

    public List<Wave> waves; // All waves in the level
    public Transform spawnPoint; // Spawn location for enemies

    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    public delegate void WaveCompletedHandler();
    public event WaveCompletedHandler OnWaveCompleted;

    public void StartWave(int waveNumber)
    {
        if (waveNumber - 1 < waves.Count)
        {
            currentWaveIndex = waveNumber - 1;
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
        else
        {
            Debug.Log("All waves completed.");
        }
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log($"Spawning wave: {wave.waveName}");
        isSpawning = true;

        foreach (var enemyInfo in wave.enemies)
        {
            for (int i = 0; i < enemyInfo.count; i++)
            {
                SpawnEnemy(enemyInfo.enemyPrefab);
                yield return new WaitForSeconds(enemyInfo.spawnDelay);
            }
        }

        isSpawning = false;
        Debug.Log($"Wave {wave.waveName} completed.");

        OnWaveCompleted?.Invoke();
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    public bool IsWaveInProgress()
    {
        return isSpawning;
    }
}