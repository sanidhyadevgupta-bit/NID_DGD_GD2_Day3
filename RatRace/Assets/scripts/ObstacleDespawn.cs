using UnityEngine;

public class ObstacleAutoDespawn : MonoBehaviour
{
    [Header("Despawn Height")]
    public float despawnY = -2f; // adjust based on belt height

    void Update()
    {
        if (transform.position.y < despawnY)
        {
            Destroy(gameObject);
        }
    }
}
