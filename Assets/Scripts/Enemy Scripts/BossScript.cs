using UnityEngine;

public class BossScript : MonoBehaviour
{
    public GameObject enemyBullet;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public int health = 10;

    void Start()
    {
        InvokeRepeating("Shoot", fireRate, fireRate);
    }

    void Shoot()
    {
        Instantiate(enemyBullet, firePoint.position, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            health--;
            Debug.Log(health);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
