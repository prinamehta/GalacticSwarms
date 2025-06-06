using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public GameObject bossPrefab;  
    public float spawnInterval = 5f; 
    public bool stopSpawning = false;
    public float enemyEntrySpeed = 2f; // Controls how fast enemies slide in
    
    // Spawn position and target position offsets
    private float spawnYOffset = 8f; // Spawn this far above the screen
    
    private int level;
    private int bossesSpawned = 0; // Track how many bosses we've spawned
    private float bossSpawnTimer = 0f; // Timer for boss spawning
    private float bossSpawnDelay = 2f; // Delay between boss spawns (if not spawning simultaneously)
    private bool spawnBossesTogether = false; // Whether to spawn bosses simultaneously or sequentially

    void Start()
    {
        // Detect the scene name to determine the level
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);
        if (sceneName == "Level 1") level = 1;
        else if (sceneName == "Level 2") level = 2;
        else if (sceneName == "Level 3") 
        {
            level = 3;
            // Randomly decide if bosses should spawn together or with a delay
            spawnBossesTogether = Random.value > 0.5f;
        }

        InvokeRepeating("SpawnEnemies", 2f, spawnInterval);
    }

    void Update()
    {
        // Only use this timer logic in Level 3 if we haven't spawned all bosses yet
        if (level == 3 && bossesSpawned < 2)
        {
            bossSpawnTimer += Time.deltaTime;

            // If first boss hasn't spawned yet or if we want to spawn them together
            if (bossesSpawned == 0)
            {
                if (bossSpawnTimer >= 2f) // Initial delay before first boss
                {
                    SpawnBoss();
                    
                    // Reset the timer for potential second boss spawn
                    bossSpawnTimer = 0f;
                    
                    // If spawning bosses together, spawn the second one immediately
                    if (spawnBossesTogether)
                    {
                        SpawnBoss();
                    }
                }
            }
            // If first boss has spawned but second hasn't yet (and we're not spawning them together)
            else if (bossesSpawned == 1 && !spawnBossesTogether)
            {
                if (bossSpawnTimer >= bossSpawnDelay)
                {
                    SpawnBoss();
                }
            }
        }
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
        // Level 3 boss spawning is now handled in Update() for better timing control
    }

    void SpawnSingleEnemy()
    {
        float randomX = Random.Range(-5f, 5f);
        float targetY = Random.Range(3f, 5f);
        
        // Spawn position (above screen)
        Vector3 spawnPosition = new Vector3(randomX, targetY + spawnYOffset, -0.55f);
        
        // Target position (where we want the enemy to stop)
        Vector3 targetPosition = new Vector3(randomX, targetY, -0.55f);
        
        // Instantiate the enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        
        // Add component to handle the sliding movement
        EnemyEntryMovement movement = enemy.AddComponent<EnemyEntryMovement>();
        movement.SetTargetPosition(targetPosition, enemyEntrySpeed);
    }

    void SpawnVFormation()
    {
        int numEnemies = 5;
        float spacing = 1.5f;

        for (int i = 0; i < numEnemies; i++)
        {
            float xOffset = (i - numEnemies / 2) * spacing;
            float yOffset = Mathf.Abs(i - numEnemies / 2) * spacing; // Creates the 'V' shape
            
            // Target position (where we want the enemy to stop)
            Vector3 targetPosition = new Vector3(xOffset, 4f - yOffset, -0.55f);
            
            // Spawn position (above screen)
            Vector3 spawnPosition = new Vector3(xOffset, targetPosition.y + spawnYOffset, -0.55f);
            
            // Instantiate the enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
            // Add component to handle the sliding movement
            EnemyEntryMovement movement = enemy.AddComponent<EnemyEntryMovement>();
            movement.SetTargetPosition(targetPosition, enemyEntrySpeed);
        }
    }

    void SpawnBoss()
    {
        if (bossesSpawned >= 2) return; // Don't spawn more than 2 bosses
        
        // Define different positions for the first and second boss
        Vector3 targetPosition;
        
        if (bossesSpawned == 0)
        {
            // First boss on the left side
            targetPosition = new Vector3(-3f, 4f, -0.55f);
        }
        else
        {
            // Second boss on the right side
            targetPosition = new Vector3(3f, 4f, -0.55f);
        }
        
        // Spawn position (above screen)
        Vector3 spawnPosition = new Vector3(targetPosition.x, targetPosition.y + spawnYOffset, -0.55f);
        
        // Instantiate the boss
        GameObject boss = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
        
        // Add component to handle the sliding movement
        EnemyEntryMovement movement = boss.AddComponent<EnemyEntryMovement>();
        movement.SetTargetPosition(targetPosition, enemyEntrySpeed * 0.5f); // Bosses move slightly slower
        
        // Increment boss counter
        bossesSpawned++;
    }
}

// New component to handle the enemy entry movement
public class EnemyEntryMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed;
    private bool reachedTarget = false;

    public void SetTargetPosition(Vector3 target, float speed)
    {
        targetPosition = target;
        moveSpeed = speed;
    }

    void Update()
    {
        // Only move if we haven't reached the target yet
        if (!reachedTarget)
        {
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            // Check if we've reached the target (with a small threshold)
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // Snap to the exact position
                reachedTarget = true;
                
                // Remove this component since we don't need it anymore
                Destroy(this);
            }
        }
    }
}