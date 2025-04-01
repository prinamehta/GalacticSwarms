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

            // Decrease player lives
            playerLives--;

            if (playerLives > 0)
            {
                Debug.Log("Player hit! Lives remaining: " + playerLives);
                return; // Don't trigger Game Over yet
            }

            // Trigger Game Over if lives run out
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
