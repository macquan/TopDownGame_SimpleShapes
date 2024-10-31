using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  
    public float spawnInterval = 5f;
    private float timeSinceLastSpawn = 0f;
    private float spawnRateIncrease = 0.95f; 
    private float minSpawnInterval = 1f;    
    public float minSpawnDistance = 6f; 

    private Transform player;       

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f;

            spawnInterval = Mathf.Max(spawnInterval * spawnRateIncrease, minSpawnInterval);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;

        do
        {
            spawnPosition = new Vector3(
                Random.Range(-10f, 10f),
                0,
                Random.Range(-10f, 10f)
            );
        } while (Vector3.Distance(spawnPosition, player.position) < minSpawnDistance);

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}
