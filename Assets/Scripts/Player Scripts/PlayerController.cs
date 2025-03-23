using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed = 5f;
    public float minX;
    public float maxX;

    void Update()
    {
        MovePlayer();
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
}
