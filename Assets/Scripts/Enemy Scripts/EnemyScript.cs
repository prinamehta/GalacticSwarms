using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private float powerUpDropChance = 0.3f;
    public float minFireRate = 3f, maxFireRate = 6f;
    public float startFireDelay = 3f;
    private float fireTimer;

    private bool canFire = false;

    void Start()
    {
        fireTimer = Random.Range(minFireRate, maxFireRate);
        Invoke("EnableFiring", startFireDelay);
    }

    void Update()
    {
        if(canFire && GameObject.FindWithTag("Player"))
        {
            Shoot();
        }
    }

    void EnableFiring()
    {
        if(GameObject.FindWithTag("Player"))
        {
            canFire = true;
        }
    }

    void Shoot()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Instantiate(enemyBullet, firePoint.position, Quaternion.identity);
            fireTimer = Random.Range(minFireRate, maxFireRate);
        }
    }

    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            if(!GameObject.FindWithTag("Player"))
            {
                Destroy(other.gameObject);
                return;
            }
            ScoreManager.instance.AddPoint();
            Destroy(other.gameObject); 
            DestroyAllEnemyBullets();
            SpawnPowerUp();
            Destroy(gameObject);
        }
    }

    void SpawnPowerUp()
    {
        if(powerUps.Length > 0 && Random.value < powerUpDropChance)
        {
            int randomIndex = Random.Range(0, powerUps.Length);
            Instantiate(powerUps[randomIndex], transform.position, Quaternion.identity);
        }
    }

    void DestroyAllEnemyBullets()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }
    }

}
