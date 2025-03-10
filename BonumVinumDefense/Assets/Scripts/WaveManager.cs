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

    private List<Enemy> aliveEnemies = new List<Enemy>();

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
            //currentWaveIndex++;
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
                SpawnEnemy(enemyInfo.enemyPrefab, currentWaveIndex);
                yield return new WaitForSeconds(enemyInfo.spawnDelay);
            }
        }

        isSpawning = false;
        Debug.Log($"Wave {wave.waveName} completed.");

        while (aliveEnemies.Count > 0)
        {
            yield return null;
        }
        currentWaveIndex++;
        ScoreManager.instance.UpdateScore();

        OnWaveCompleted?.Invoke(currentWaveIndex);
    }

    private void SpawnEnemy(GameObject enemyPrefab, int waveNumber)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            aliveEnemies.Add(enemyComponent);
            enemyComponent.ScaleStats(waveNumber);
            enemyComponent.OnDeathEvent += EnemyDied;
            enemyComponent.target = enemyTarget;
            enemyComponent.OnReachGoal += EnemyReachedGoal;
        }
    }

    private void EnemyDied(Enemy enemy)
    {
        aliveEnemies.Remove(enemy);
    }

    private void EnemyReachedGoal(Enemy enemy)
    {
        aliveEnemies.Remove(enemy);
        Debug.Log($"Enemy reached goal! {aliveEnemies} enemies left.");
        GameManager.Instance.LoseLife(enemy.Damage); // Tell GameManager to remove life
    }

    public bool IsWaveInProgress()
    {
        return isSpawning || aliveEnemies.Count > 0;
    }

    public int GetCurrentWaveNumber()
    {
        return currentWaveIndex + 1;
    }

}
