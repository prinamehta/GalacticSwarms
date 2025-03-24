using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public float deactivateTimer = 3f;

    void Start()
    {
        Invoke("DeactivateGameObject", deactivateTimer);
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 temp = transform.position;
        temp.y -= speed * Time.deltaTime; 
        transform.position = temp;
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
