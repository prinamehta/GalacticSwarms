using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public GameObject bossPrefab;  
    public float spawnInterval = 5f; 
    public bool stopSpawning = false;

    private int level;

    void Start()
    {
        // Detect the scene name to determine the level
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);
        if (sceneName == "Level 1") level = 1;
        else if (sceneName == "Level 2") level = 2;
        else if (sceneName == "Level 3") level = 3;

        InvokeRepeating("SpawnEnemies", 2f, spawnInterval);
    }

    void SpawnEnemies()
    {
        if (stopSpawning || !GameObject.FindWithTag("Player")) return;

        if (level == 1)
        {
            SpawnSingleEnemy();
        }
        else if (level == 2)
        {
            SpawnVFormation();
        }
        else if (level == 3)
        {
            SpawnBoss();
        }
    }

    void SpawnSingleEnemy()
    {
        float randomX = Random.Range(-5f, 5f);
        float randomY = Random.Range(3f, 5f);
        Vector3 spawnPosition = new Vector3(randomX, randomY, -0.55f);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnVFormation()
    {
        int numEnemies = 5;
        float spacing = 1.5f;

        for (int i = 0; i < numEnemies; i++)
        {
            float xOffset = (i - numEnemies / 2) * spacing;
            float yOffset = Mathf.Abs(i - numEnemies / 2) * spacing; // Creates the 'V' shape
            Vector3 spawnPosition = new Vector3(xOffset, 4f - yOffset, -0.55f);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnBoss()
    {
        if (!GameObject.FindWithTag("Boss"))
        {
            Vector3 spawnPosition = new Vector3(0, 4f, -0.55f);
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
