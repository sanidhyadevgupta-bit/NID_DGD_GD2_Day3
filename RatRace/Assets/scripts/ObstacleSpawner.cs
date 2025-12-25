using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 3f;

    void Start()
    {
        ScheduleNext();
    }

    void Spawn()
    {
        Instantiate(obstaclePrefab, transform.position, Quaternion.identity);
        ScheduleNext();
    }

    void ScheduleNext()
    {
        Invoke(nameof(Spawn), Random.Range(minSpawnTime, maxSpawnTime));
    }
}
