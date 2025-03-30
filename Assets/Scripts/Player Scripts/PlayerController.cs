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

    private bool isDead;
    public GameManagerScript gameManager;

    AudioManager audioManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
                audioManager.PlaySFX(audioManager.playerShooting);
                canAttack = false;
                attackTimer = 0f;
                Instantiate(playerBullet, attackPoint.position, Quaternion.identity);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyBullet") && !isDead)
        {
            if(!GameObject.FindWithTag("Enemy"))
            {
                Destroy(gameObject);
                return;
            }
            Destroy(other.gameObject);
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
