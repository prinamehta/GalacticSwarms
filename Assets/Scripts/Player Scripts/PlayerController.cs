using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float minX, maxX;
    [SerializeField] private GameObject playerBullet;
    [SerializeField] private Transform attackPoint;
    
    // Reference to the PlayerScript that contains playerLives
    [SerializeField] private PlayerScript playerScript;

    public float attackCooldown = 0.35f;
    private float currentAttackTimer;
    private bool canAttack = true;
    private float originalFireRate;
    private bool isShielded = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool hasFireRateBoost = false;

    private bool isDead;
    public GameManagerScript gameManager;

    AudioManager audioManager;
    
    void Start()
    {
        currentAttackTimer = 0f;
        originalFireRate = attackCooldown;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        
        // Find PlayerScript if not assigned
        if (playerScript == null)
        {
            playerScript = GetComponent<PlayerScript>();
            
            // If it's not on the same GameObject, try to find it in the scene
            if (playerScript == null)
            {
                playerScript = FindObjectOfType<PlayerScript>();
                
                if (playerScript == null)
                {
                    Debug.LogError("PlayerScript not found! Please assign it in the inspector.");
                }
            }
        }
    }

    void Update()
    {
        MovePlayer();
        Attack();
    }

    void MovePlayer()
    {
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            Vector3 temp = transform.position;
            temp.x += speed * Time.deltaTime;

            if (temp.x > maxX)
                temp.x = maxX;

            transform.position = temp;

        } else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            Vector3 temp = transform.position;
            temp.x -= speed * Time.deltaTime;

            if (temp.x < minX)
                temp.x = minX;

            transform.position = temp;
        }
    }

    void Attack()
    {
        if (!canAttack)
        {
            currentAttackTimer += Time.deltaTime;
            if (currentAttackTimer >= attackCooldown)
            {
                canAttack = true;
                currentAttackTimer = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && canAttack)
        {
            if(canAttack)
            {
                audioManager.PlaySFX(audioManager.playerShooting);
                canAttack = false;
                Instantiate(playerBullet, attackPoint.position, Quaternion.identity);
            }

            canAttack = false;
            currentAttackTimer = 0f;
            Instantiate(playerBullet, attackPoint.position, Quaternion.identity);
        }
    }

    public void ActivateShield(float duration)
    {
        if (isShielded) 
        {
            // Extend duration if already shielded
            CancelInvoke("DeactivateShield");
            Invoke("DeactivateShield", duration);
            return;
        }
        
        isShielded = true;
        
        // Visual feedback - turn blue for shield
        UpdatePlayerVisuals();
        
        Invoke("DeactivateShield", duration);
        Debug.Log("Shield activated!");
    }

    void DeactivateShield()
    {
        isShielded = false;
        
        // Update visual based on other active power-ups
        UpdatePlayerVisuals();
        
        Debug.Log("Shield deactivated.");
    }

    public void IncreaseFireRate(float multiplier, float duration)
    {
        // Cancel any pending reset
        CancelInvoke("ResetFireRate");
        
        // Store original value if not already modified
        if (attackCooldown == originalFireRate)
        {
            attackCooldown /= multiplier;
        }
        
        hasFireRateBoost = true;
        
        // Update visual based on power-up state
        UpdatePlayerVisuals();
        
        Debug.Log("Fire rate increased! New cooldown: " + attackCooldown);
        Invoke("ResetFireRate", duration);
    }

    void ResetFireRate()
    {
        attackCooldown = originalFireRate;
        hasFireRateBoost = false;
        
        // Update visual based on other active power-ups
        UpdatePlayerVisuals();
        
        Debug.Log("Fire rate reset.");
    }

    // Update player color based on active power-ups
    void UpdatePlayerVisuals()
    {
        if (spriteRenderer == null) return;
        
        if (isShielded)
        {
            // Blue for shield (priority over fire rate)
            spriteRenderer.color = Color.cyan;
        }
        else if (hasFireRateBoost)
        {
            // Green for fire rate
            spriteRenderer.color = Color.green;
        }
        else
        {
            // Reset to original color if no power-ups active
            spriteRenderer.color = originalColor;

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet") && !isDead)
        {
            if (isShielded)
            {
                // Shield blocks the bullet
                Destroy(other.gameObject);
                Debug.Log("Bullet blocked by shield");
                return;
            }
            
            Destroy(other.gameObject);

            // Decrease player lives - using playerScript reference
            if (playerScript != null)
            {
                playerScript.playerLives--;

                if (playerScript.playerLives > 0)
                {
                    Debug.Log("Player hit! Lives remaining: " + playerScript.playerLives);
                    return; // Don't trigger Game Over yet
                }
            }
            else
            {
                Debug.LogError("PlayerScript reference is missing!");
            }

            // Trigger Game Over if lives run out
            Destroy(gameObject);
            isDead = true;
            gameObject.SetActive(false);
            if (audioManager != null && audioManager.musicSource != null)
            {
                audioManager.musicSource.Stop();
            }
            audioManager.PlaySFX(audioManager.death);
            gameManager.gameOver();

            Debug.Log("You Are Dead");
            
        }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
}