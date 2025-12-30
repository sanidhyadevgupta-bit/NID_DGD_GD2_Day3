using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject obstaclePrefab;

    [Header("Spawn Timing")]
    public float spawnInterval = 2f;
    private float timer = 0f;

    [Header("Spawn Points (X/Y only used)")]
    public Transform[] spawnPoints;

    [Header("Forced Z Lock")]
    public float forcedZ = 0f; // The Z position obstacles will always use

    [Header("Variable Scaling")]
    public Vector2 xScaleRange = new Vector2(0.5f, 2f);
    public Vector2 yScaleRange = new Vector2(0.5f, 3f);

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Spawner missing prefab or spawn points.");
            return;
        }

        // Choose random spawn point (for X & Y only)
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject obj = Instantiate(obstaclePrefab, sp.position, Quaternion.identity);
        AudioManager.Instance.PlaySpawn();

        // Lock Z to fixed value
        obj.transform.position = new Vector3(sp.position.x, sp.position.y, forcedZ);

        // Random 2D scale
        float scaleX = Random.Range(xScaleRange.x, xScaleRange.y);
        float scaleY = Random.Range(yScaleRange.x, yScaleRange.y);
        obj.transform.localScale = new Vector3(scaleX, scaleY, 1f);

        // Naming for debugging
        obj.name = $"Obstacle_{scaleX:F2}x{scaleY:F2}";
    }
}
