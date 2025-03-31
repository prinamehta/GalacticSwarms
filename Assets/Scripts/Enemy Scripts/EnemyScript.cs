using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject enemyBullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject explosionEffect;
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
            TriggerExplosion();
            Destroy(other.gameObject); 
            DestroyAllEnemyBullets();
            Destroy(gameObject);
        }
    }

    void TriggerExplosion()
    {
        if(explosionEffect!= null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
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
