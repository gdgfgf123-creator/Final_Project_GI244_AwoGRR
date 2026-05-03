using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject[] enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Setting")]
    public int waveNumber = 1;
    public int maxWaves = 5;
    public int[] waveEnemyCount = { 3, 5, 10, 15, 20 };

    [Header("Timing")]
    public float spawnDelay = 0.5f;
    public float timeBetweenWaves = 3f;

    [Header("UI")]
    public TMPro.TextMeshProUGUI waveText;
    public TMPro.TextMeshProUGUI enemyText;


    private int enemiesAlive = 0;
    private bool isSpawning = false;

    void Start()
    {
        UpdateUI(); 
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        Debug.Log("?? Wave " + waveNumber + " Start!");

        isSpawning = true;

        int spawnCount = (waveNumber - 1 < waveEnemyCount.Length)
            ? waveEnemyCount[waveNumber - 1]
            : 5;

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        int randPoint = Random.Range(0, spawnPoints.Length);
        int randEnemy = Random.Range(0, enemyPrefab.Length);

        GameObject enemy = Instantiate(
            enemyPrefab[randEnemy],
            spawnPoints[randPoint].position,
            Quaternion.identity
        );

        enemiesAlive++;
        UpdateUI(); 

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.spawner = this;
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
        UpdateUI(); 

        if (enemiesAlive <= 0 && !isSpawning)
        {
            if (waveNumber >= maxWaves)
            {
                Debug.Log("?? All Waves Completed!");
                return;
            }

            waveNumber++;
            UpdateUI(); 
            StartCoroutine(StartWave());
        }
    }

    void UpdateUI()
    {
        if (waveText != null)
            waveText.text = "Wave : " + waveNumber + " / " + maxWaves;

        if (enemyText != null)
            enemyText.text = "Enemy : " + enemiesAlive;
    }
}