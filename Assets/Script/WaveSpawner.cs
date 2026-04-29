using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int waveNumber = 1;
    public int enemiesPerWave = 5;

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

        int spawnCount = enemiesPerWave + (waveNumber * 2); // ?? เพิ่มจำนวนตาม Wave

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

        GameObject enemy = Instantiate(enemyPrefab, spawnPoints[rand].position, Quaternion.identity);

        enemiesAlive++;

        // ?? บอก Enemy ให้แจ้งตอนตาย
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
            waveNumber++;
            StartCoroutine(StartWave());
        }
    }
}