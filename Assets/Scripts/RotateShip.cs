using UnityEngine;

public class RotateShip : MonoBehaviour
{
     public float rotationSpeed = 150f; // Speed of rotation

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
    }
}
