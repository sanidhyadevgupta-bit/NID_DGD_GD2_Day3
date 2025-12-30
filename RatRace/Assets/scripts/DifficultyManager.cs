using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Timeline (seconds)")]
    public float stage1Duration = 30f;  // Early life
    public float stage2Duration = 60f;  // Adulthood / struggle
    public float stage3Duration = 60f;  // Slowdown / acceptance

    private float totalTime;
    private float elapsed = 0f;

    [Header("Conveyor Settings")]
    public ConveyorBelt belt;
    public float startSpeed = 3f;
    public float maxSpeed = 10f;
    public float endSpeed = 0f;

    // belt direction flip frequency
    public float startDirectionInterval = 3f;
    public float minDirectionInterval = 0.8f;
    public float endDirectionInterval = 5f;

    [Header("Spawner Settings")]
    public ObstacleSpawner spawner;
    public float startSpawnInterval = 3f;
    public float minSpawnInterval = 0.8f;
    public float endSpawnInterval = 3.5f;

    [Header("Player Settings")]
    public PlayerController player;
    public float startMoveSpeed = 8f;
    public float peakMoveSpeed = 10f;
    public float endMoveSpeed = 2f;

    public float startJumpForce = 9f;
    public float peakJumpForce = 12f;
    public float endJumpForce = 4f;

    [Header("Manager")]
    public GameManager gameManager; // optional but recommended

    void Start()
    {
        totalTime = stage1Duration + stage2Duration + stage3Duration;

        // Initialize systems to starting values
        if (belt != null)
        {
            belt.beltSpeed = startSpeed;
            belt.changeInterval = startDirectionInterval;
        }

        if (spawner != null)
        {
            spawner.spawnInterval = startSpawnInterval;
        }

        if (player != null)
        {
            player.moveSpeed = startMoveSpeed;
            player.jumpForce = startJumpForce;
        }
    }

    void Update()
    {
        elapsed += Time.deltaTime;

        // Stage 1: Warm-up (0 to stage1Duration)
        if (elapsed <= stage1Duration)
        {
            float t = elapsed / stage1Duration;

            if (belt != null)
            {
                belt.beltSpeed = Mathf.Lerp(startSpeed, (startSpeed + maxSpeed) * 0.5f, t);
                belt.changeInterval = Mathf.Lerp(startDirectionInterval, minDirectionInterval * 2f, t);
            }

            if (spawner != null)
            {
                spawner.spawnInterval = Mathf.Lerp(startSpawnInterval, (startSpawnInterval + minSpawnInterval) * 0.5f, t);
            }

            if (player != null)
            {
                player.moveSpeed = Mathf.Lerp(startMoveSpeed, peakMoveSpeed, t);
                player.jumpForce = Mathf.Lerp(startJumpForce, peakJumpForce, t);
            }
        }
        // Stage 2: Peak Difficulty (middle life: stage1Duration → stage1+stage2)
        else if (elapsed <= stage1Duration + stage2Duration)
        {
            float t = (elapsed - stage1Duration) / stage2Duration;

            if (belt != null)
            {
                belt.beltSpeed = Mathf.Lerp((startSpeed + maxSpeed) * 0.5f, maxSpeed, t);
                belt.changeInterval = Mathf.Lerp(minDirectionInterval * 2f, minDirectionInterval, t);
            }

            if (spawner != null)
            {
                spawner.spawnInterval = Mathf.Lerp((startSpawnInterval + minSpawnInterval) * 0.5f, minSpawnInterval, t);
            }

            if (player != null)
            {
                player.moveSpeed = Mathf.Lerp(peakMoveSpeed, (peakMoveSpeed + endMoveSpeed) * 0.5f, t);
                player.jumpForce = Mathf.Lerp(peakJumpForce, (peakJumpForce + endJumpForce) * 0.5f, t);
            }
        }
        // Stage 3: Slowdown (late life: final minute)
        else if (elapsed <= totalTime)
        {
            float t = (elapsed - stage1Duration - stage2Duration) / stage3Duration;

            if (belt != null)
            {
                belt.beltSpeed = Mathf.Lerp(maxSpeed, endSpeed, t);
                belt.changeInterval = Mathf.Lerp(minDirectionInterval, endDirectionInterval, t);
            }

            if (spawner != null)
            {
                spawner.spawnInterval = Mathf.Lerp(minSpawnInterval, endSpawnInterval, t);
            }

            if (player != null)
            {
                player.moveSpeed = Mathf.Lerp((peakMoveSpeed + endMoveSpeed) * 0.5f, endMoveSpeed, t);
                player.jumpForce = Mathf.Lerp((peakJumpForce + endJumpForce) * 0.5f, endJumpForce, t);
            }
        }
        // END OF LIFE / GAME COMPLETE
        else
        {
            if (belt != null)
            {
                belt.beltSpeed = 0f;
                belt.changeInterval = Mathf.Infinity;
            }

            if (spawner != null)
            {
                spawner.spawnInterval = 999f; // Stops spawns
            }

            if (player != null)
            {
                player.moveSpeed = 0f;
                player.jumpForce = 0f;
            }

            if (gameManager != null)
            {
                GameManager.Instance.TriggerGameEnd();
            }

            enabled = false; // Stop updating
        }
    }
}
