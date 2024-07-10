using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;

    private int currentRoundIndex = 0;
    private GameManager gameManager;
    private Coroutine spawnCoroutine;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnRound());
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    public bool HasMoreRounds()
    {
        return currentRoundIndex < waves.Length;
    }

    IEnumerator SpawnRound()
    {
        if (currentRoundIndex < waves.Length)
        {
            Wave currentWave = waves[currentRoundIndex];
            int totalEnemies = GetTotalEnemyCount(currentWave);
            gameManager.SetRemainingEnemies(totalEnemies);

            for (int i = 0; i < currentWave.enemyPrefab.Length; i++)
            {
                for (int j = 0; j < currentWave.enemyCounts[i]; j++)
                {
                    if (gameManager.currentState != GameManager.GameState.Active)
                    {
                        yield return new WaitUntil(() => gameManager.currentState == GameManager.GameState.Active);
                    }

                    GameObject enemy = SpawnEnemy(currentWave.enemyPrefab[i]);
                    spawnedEnemies.Add(enemy);
                    gameManager.IncrementRemainingEnemies();
                    yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
                }
            }

            currentRoundIndex++;
            yield return new WaitUntil(() => gameManager.currentState == GameManager.GameState.Idle);
        }

        if (currentRoundIndex >= waves.Length && spawnedEnemies.Count == 0)
        {
            gameManager.GameOver();
        }

        spawnCoroutine = null;
    }

    GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        if (enemy.TryGetComponent<Enemy>(out var enemyScript))
        {
            enemyScript.OnDestroyed += () => EnemyDefeated(enemy);
        }

        return enemy;
    }

    int GetTotalEnemyCount(Wave wave)
    {
        int total = 0;
        foreach (int count in wave.enemyCounts)
        {
            total += count;
        }
        return total;
    }

    public void EnemyDefeated(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
        gameManager.EnemyDefeated();

        if (currentRoundIndex >= waves.Length && spawnedEnemies.Count == 0)
        {
            gameManager.GameOver();
        }
    }
}

[System.Serializable]
public class Wave
{
    public GameObject[] enemyPrefab;
    public int[] enemyCounts;
    public float timeBetweenSpawns = 1f;
}