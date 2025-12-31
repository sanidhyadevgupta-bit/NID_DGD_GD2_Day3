using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("Prefab")]
    public GameObject obstaclePrefab;

    [Header("Spawn Timing")]
    public float spawnInterval = 3f;
    float timer = 0f;

    [Header("Scale (set by DifficultyManager)")]
    public float minScale = 0.4f;
    public float maxScale = 1.6f;

    void Update()
    {
        if (Time.timeScale == 0f) return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        if (spawnPoints.Length == 0) return;

        int r = Random.Range(0, spawnPoints.Length);
        Transform p = spawnPoints[r];

        GameObject obj = Instantiate(obstaclePrefab, p.position, Quaternion.identity);

        // Apply growth from DifficultyManager (minScale and maxScale updated there)
        float scaleX = Random.Range(minScale, maxScale);
        float scaleY = Random.Range(minScale, maxScale * 1.2f);
        obj.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        // Optional logging for debugging
        // Debug.Log("Spawn with scale: " + obj.transform.localScale);
    }
}
