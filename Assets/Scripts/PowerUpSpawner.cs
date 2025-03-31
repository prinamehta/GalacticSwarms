using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] powerUpPrefabs; // Assign shield and fire rate power-up prefabs
    [SerializeField] private float spawnInterval = 5f;    // How often to spawn
    [SerializeField] private float minX, maxX;            // Spawn position range
    [SerializeField] private float spawnY = 8f;           // Spawn height
    
    private float spawnTimer;
    
    void Start()
    {
        spawnTimer = spawnInterval; // Spawn first power-up after interval
    }
    
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        
        if (spawnTimer <= 0f)
        {
            SpawnRandomPowerUp();
            spawnTimer = spawnInterval;
        }
    }
    
    void SpawnRandomPowerUp()
    {
        if (powerUpPrefabs.Length == 0)
        {
            Debug.LogError("No power-up prefabs assigned to spawner!");
            return;
        }
        
        // Choose random power-up type
        int randomIndex = Random.Range(0, powerUpPrefabs.Length);
        
        // Choose random position
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, -0.715f);
        
        // Spawn the power-up
        Instantiate(powerUpPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        
        Debug.Log("Spawned power-up at " + spawnPosition);
    }
}