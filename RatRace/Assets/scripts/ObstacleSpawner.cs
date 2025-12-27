using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.75f;
    public float treadmillSpeed = 3f;
    public Vector2 heightRange = new Vector2(0.5f, 1.5f); // for variety

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0)
        {
            Debug.LogWarning("No obstacles assigned!");
            return;
        }

        Vector3 spawnPos = transform.position;
        spawnPos.y += Random.Range(heightRange.x, heightRange.y);

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        GameObject ob = Instantiate(prefab, spawnPos, Quaternion.identity);

        ObstacleMover mover = ob.GetComponent<ObstacleMover>();
        if (!mover) mover = ob.AddComponent<ObstacleMover>();

        mover.speed = treadmillSpeed;
        ob.tag = "Obstacle";
    }
}
