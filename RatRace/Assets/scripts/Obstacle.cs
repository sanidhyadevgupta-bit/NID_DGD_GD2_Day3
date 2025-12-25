using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float treadmillSpeed = 2.5f;

    void Update()
    {
        transform.Translate(Vector3.left * treadmillSpeed * Time.deltaTime, Space.World);
    }
}
