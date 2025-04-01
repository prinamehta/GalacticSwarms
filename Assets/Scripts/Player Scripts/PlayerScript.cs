using UnityEngine;
using UnityEngine.UI; // Add this for UI elements

public class PlayerScript : MonoBehaviour
{
    public int playerLives = 3;  // Set initial number of lives
    public int maxLives = 3;     // Maximum number of lives
    public GameObject gameOverUI;
    public GameManagerScript gameManager;
    
    // UI References
    public Text livesText;       // Text to display lives count
    // OR if using TextMeshPro
    // public TMPro.TextMeshProUGUI livesText;
    
    // Optional: Life icon images
    public GameObject[] lifeIcons; // Array of 3 life icon GameObjects
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManagerScript>();
        UpdateLivesUI();
    }

    // This function will be called when the player is hit by an enemy or enemy bullet
    public void TakeDamage()
    {
        playerLives--;
        UpdateLivesUI();

        if (playerLives <= 0)
        {
            // Trigger Game Over if lives run out
            Die();
        }
        else
        {
            // Handle player getting hit, maybe play an animation, sound, etc.
            PlayHitAnimation();
            Debug.Log("Player was hit! Lives remaining: " + playerLives);
        }
    }
    
    private void UpdateLivesUI()
    {
        // Update text display if using text
        if (livesText != null)
        {
            livesText.text = "Lives: " + playerLives;
        }
        
        // Update life icons if using those
        if (lifeIcons != null && lifeIcons.Length > 0)
        {
            for (int i = 0; i < lifeIcons.Length; i++)
            {
                if (i < playerLives)
                    lifeIcons[i].SetActive(true);
                else
                    lifeIcons[i].SetActive(false);
            }
        }
    }
    
    private void PlayHitAnimation()
    {

        
        // Basic example: flash the player by toggling renderer
        StartCoroutine(FlashCoroutine());
    }
    
    private System.Collections.IEnumerator FlashCoroutine()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            for (int i = 0; i < 3; i++)
            {
                renderer.enabled = false;
                yield return new WaitForSeconds(0.1f);
                renderer.enabled = true;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    
    private void Die()
    {
        Debug.Log("Player is dead!");
        // Disable player controls/movement
        GetComponent<Collider2D>().enabled = false;
    
    }
}