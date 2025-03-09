using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public List<Wave> baseWaves;
    public Transform spawnPoint;
    public Transform enemyTarget;
    public GameObject enemyPrefab;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;
    private int aliveEnemies = 0;

    public delegate void WaveCompletedHandler(int waveNumber);
    public event WaveCompletedHandler OnWaveCompleted;

    public void StartInfiniteWaves()
    {
        Debug.Log("Starting infinite waves.");
        if (baseWaves == null || baseWaves.Count == 0)
        {
            Debug.LogWarning("Base waves list is empty! Creating a default wave.");
            CreateDefaultBaseWave();
        }

        StartCoroutine(SpawnInfiniteWaves());
    }

    private void CreateDefaultBaseWave()
    {
        baseWaves = new List<Wave>();

        Wave defaultWave = new Wave
        {
            waveName = "Default Wave",
            enemies = new List<EnemySpawnInfo>
            {
                new EnemySpawnInfo
                {
                    enemyPrefab = enemyPrefab,
                    count = 5,
                    spawnDelay = 1.5f
                }
            }
        };

        baseWaves.Add(defaultWave);
    }


    private IEnumerator SpawnInfiniteWaves()
    {
        while (true)
        {
            Wave newWave = GenerateWave(currentWaveIndex + 1);
            yield return StartCoroutine(SpawnWave(newWave));
            currentWaveIndex++;
            yield return new WaitForSeconds(5f);
        }
    }

    private Wave GenerateWave(int waveNumber)
    {
        Wave baseWave = baseWaves[waveNumber % baseWaves.Count];
        Wave generatedWave = new Wave { waveName = $"Wave {waveNumber}", enemies = new List<EnemySpawnInfo>() };

        foreach (var enemyInfo in baseWave.enemies)
        {
            EnemySpawnInfo newEnemyInfo = new EnemySpawnInfo
            {
                enemyPrefab = enemyInfo.enemyPrefab,
                count = enemyInfo.count + (waveNumber / 2), // Increase enemy count gradually
                spawnDelay = Mathf.Max(0.1f, enemyInfo.spawnDelay * 0.95f) // Reduce spawn delay over time
            };
            generatedWave.enemies.Add(newEnemyInfo);
        }
        return generatedWave;
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

        while (aliveEnemies > 0)
        {
            yield return null;
        }

        OnWaveCompleted?.Invoke(currentWaveIndex + 1);
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        aliveEnemies++;
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.OnDeathEvent += EnemyDied;
            enemyComponent.target = enemyTarget;
            enemyComponent.OnReachGoal += EnemyReachedGoal;
        }
    }

    private void EnemyDied()
    {
        aliveEnemies--;
    }

    private void EnemyReachedGoal(Enemy enemy)
    {
        aliveEnemies--; // Reduce count when enemy reaches goal
        Debug.Log($"Enemy reached goal! {aliveEnemies} enemies left.");
        GameManager.Instance.LoseLife(enemy.Damage); // Tell GameManager to remove life
    }

    public bool IsWaveInProgress()
    {
        return isSpawning || aliveEnemies > 0;
    }
}
