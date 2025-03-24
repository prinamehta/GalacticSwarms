using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f; 
    public float minX, maxX, minY, maxY; 
    public bool stopSpawning = false;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2f, spawnInterval); 
    }

    void SpawnEnemy()
    {
        if(stopSpawning)
            return;

        if(GameObject.FindWithTag("Player"))
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            Vector3 spawnPosition = new Vector3(randomX, randomY, -0.55f);

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        } else
        {
            stopSpawning = true;
            CancelInvoke("SpawnEnemy");
        }           
    }
}
