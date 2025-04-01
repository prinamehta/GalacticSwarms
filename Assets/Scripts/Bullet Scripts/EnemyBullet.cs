using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float speed = 5f;
    
    void Update()
    {
        // Move bullet downward (assuming player is at bottom)
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        
        // Destroy bullet if it goes off screen
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the PlayerScript component and call TakeDamage()
            PlayerScript player = other.GetComponent<PlayerScript>();
            if (player != null)
            {
                player.TakeDamage(); // Deduct one life from the player
            }
            
            // Destroy the bullet on hit
            Destroy(gameObject);
        }
    }
}