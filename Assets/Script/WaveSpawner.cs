using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Setting")]
    public int waveNumber = 1;
    public int maxWaves = 5;

    // ?? กำหนดจำนวนศัตรูแต่ละ Wave เอง
    public int[] waveEnemyCount = { 3, 5, 10, 15, 20 };

    [Header("Timing")]
    public float spawnDelay = 0.5f;
    public float timeBetweenWaves = 3f;

    private int enemiesAlive = 0;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(StartWave());
    }

    IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        Debug.Log("Wave " + waveNumber + " Start!");

        isSpawning = true;

        int spawnCount = 0;

        // ?? เช็คว่ามีค่าที่กำหนดไว้ไหม
        if (waveNumber - 1 < waveEnemyCount.Length)
        {
            spawnCount = waveEnemyCount[waveNumber - 1];
        }
        else
        {
            spawnCount = 5; // fallback ถ้าเกิน array
        }

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnDelay);
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        int rand = Random.Range(0, spawnPoints.Length);

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoints[rand].position,
            Quaternion.identity
        );

        enemiesAlive++;

        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.spawner = this;
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && !isSpawning)
        {
            if (waveNumber >= maxWaves)
            {
                Debug.Log("?? All Waves Completed!");
                return;
            }

            waveNumber++;
            StartCoroutine(StartWave());
        }
    }
}