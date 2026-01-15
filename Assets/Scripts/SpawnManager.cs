using System.Collections;
using UnityEngine;

[System.Serializable]
public struct EnemySpawn
{
    public GameObject enemyPrefab;
    public int count;       
}

[System.Serializable]
public struct Wave
{
    public EnemySpawn[] enemyTypes;
    public float timeBetweenSpawns;
}


public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; private set; }

    [Header("Path Setup")]
    public Transform entryPoint;
    public Transform[] pathPoints;

    [Header("Wave Setup")]
    public Wave[] waves;
    public int currentWaveIndex = 0;
    private int enemiesRemaining = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void StartNextWave()
    {
    
        StartCoroutine(SpawnWave());
    }

    public IEnumerator SpawnWave()
    {
        if (currentWaveIndex >= waves.Length)
        {
            CheckForWinCondition();
            yield break;
        }

        Wave currentWave = waves[currentWaveIndex];
        int totalEnemiesInWave = 0;

        foreach (var enemyType in currentWave.enemyTypes)
        {
            totalEnemiesInWave += enemyType.count;
        }

        enemiesRemaining = totalEnemiesInWave;
        Debug.Log("Total enemies in Wave " + (currentWaveIndex + 1) + ": " + enemiesRemaining);

        foreach (var enemyType in currentWave.enemyTypes)
        {
            for (int i = 0; i < enemyType.count; i++)
            {
                SpawnEnemy(enemyType.enemyPrefab);
                yield return new WaitForSeconds(currentWave.timeBetweenSpawns);
            }
        }

    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemyObject = Instantiate(enemyPrefab, entryPoint.position, Quaternion.identity);

        EnemyBase enemyComponent = enemyObject.GetComponent<EnemyBase>();
        if (enemyComponent != null)
        {
            enemyComponent.Init(pathPoints);
        }
    }

    public void EnemyDefeated()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            LogManager.Instance.WriteLog($"Dalga {currentWaveIndex + 1} temizlendi!");
            currentWaveIndex++;

            if (currentWaveIndex < waves.Length)
            {
                GameManager.Instance.StartNextWave();
            }
            else 
            {
                CheckForWinCondition();
            }
        }
    }

    private void CheckForWinCondition()
    {
        if (currentWaveIndex >= waves.Length && enemiesRemaining <= 0)
        {
            GameManager.Instance.GameOver(true);
             
        }
    }
}