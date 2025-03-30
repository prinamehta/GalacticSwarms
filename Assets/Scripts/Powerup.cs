using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Shield, FireRate }
    public PowerUpType powerUpType;
    public float fallSpeed = 2f; 

    void Update()
    {
        // Make power-up fall down
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
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
            player.ActivateShield(50f); // Shield lasts for 5 seconds
        }
        else if (powerUpType == PowerUpType.FireRate)
        {
            player.IncreaseFireRate(2f, 50f); // Double fire rate for 5 seconds
        }
    }
}
