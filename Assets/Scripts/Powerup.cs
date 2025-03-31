using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Shield, FireRate }
    public PowerUpType powerUpType;
    public float fallSpeed = 2f;
    public float duration = 5f; // Duration in seconds
    
    void Update()
    {
        // Make power-up fall down
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        
        // Destroy if it goes off-screen
        if (transform.position.y < -10f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                ApplyPowerUp(player);
            }

            // Destroy power-up after collection
            Destroy(gameObject);
        }
    }

    void ApplyPowerUp(PlayerController player)
    {
        if (powerUpType == PowerUpType.Shield)
        {
            player.ActivateShield(duration);
        }
        else if (powerUpType == PowerUpType.FireRate)
        {
            player.IncreaseFireRate(2f, duration);
        }
    }
}