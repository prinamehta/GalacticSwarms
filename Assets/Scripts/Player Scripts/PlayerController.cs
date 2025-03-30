using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float minX, maxX;
    [SerializeField] private GameObject playerBullet;

    [SerializeField] private Transform attackPoint;

    public float attackTimer = 0.35f;
    private float currentAttackTimer;
    private bool canAttack;
    private float fireRate = 1f;
    private float originalFireRate;
    private bool isShielded = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor; 

    void Start()
    {
        currentAttackTimer = attackTimer;
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
        attackTimer += Time.deltaTime;
        if(attackTimer > currentAttackTimer)
        {
            canAttack = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(canAttack)
            {
                canAttack = false;
                attackTimer = 0f;
                Instantiate(playerBullet, attackPoint.position, Quaternion.identity);
            }
        }
    }

    public void ActivateShield(float duration)
    {
        if (isShielded) return; // Prevent multiple shields
        isShielded = true;
        Invoke("DeactivateShield", duration);
        Debug.Log("Shield activated!");
    }

    void DeactivateShield()
    {
        isShielded = false;
        Debug.Log("Shield deactivated.");
    }

    public void IncreaseFireRate(float multiplier, float duration)
    {
        fireRate /= multiplier;
        Debug.Log("Fire rate increased! " + fireRate);
        Invoke("ResetFireRate", duration);
    }

    void ResetFireRate()
    {
        fireRate = originalFireRate;
        Debug.Log("Fire rate reset.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            if(!GameObject.FindWithTag("Enemy"))
            {
                Destroy(gameObject);
                return;
            }
            Destroy(other.gameObject);
            Debug.Log("Bullet blocked by shield");
            Destroy(gameObject);
            GameOver();
        }
    }
    
    // TODO: Replace with GameOver Screen
    void GameOver()
    {
        Debug.Log("Game Over");
    }
}
