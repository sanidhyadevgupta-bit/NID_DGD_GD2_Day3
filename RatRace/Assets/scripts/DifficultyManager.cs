using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Phase Durations")]
    public float easyTime = 30f;
    public float rampTime = 60f;
    public float peakTime = 30f;
    public float decayTime = 30f;

    private float timer = 0f;

    [Header("Player")]
    public PlayerController player;
    public float playerMinSpeed = 3f;
    public float playerMaxSpeed = 9f;
    public float playerMinJump = 6f;
    public float playerMaxJump = 12f;

    [Header("Spawning")]
    public ObstacleSpawner spawner;
    public float spawnSlow = 2.5f;
    public float spawnFast = 0.6f;

    [Header("Obstacle Scaling")]
    public float obstacleMinScale = 0.4f;
    public float obstacleMaxScale = 1.6f;

    [Header("Belt")]
    public ConveyorBelt belt;
    public float beltSlow = 2f;
    public float beltFast = 12f;

    [Header("End")]
    public GameManager gameManager;

    bool started = false;

    public void BeginDifficulty()
    {
        timer = 0f;
        started = true;
    }

    void Update()
    {
        if (!started || Time.timeScale == 0f) return;

        timer += Time.deltaTime;

        // PHASE 1: EASY
        if (timer <= easyTime)
        {
            float t = timer / easyTime;
            ApplyDifficulty(
                Mathf.Lerp(playerMinSpeed, playerMinSpeed * 1.2f, t),
                Mathf.Lerp(playerMinJump,  playerMinJump * 1.2f,  t),
                Mathf.Lerp(spawnSlow,      spawnSlow * 0.8f,     t),
                Mathf.Lerp(beltSlow,       beltSlow * 1.2f,      t)
            );
            SetScale(0f);
            return;
        }

        // PHASE 2: RAMP
        if (timer <= easyTime + rampTime)
        {
            float t = (timer - easyTime) / rampTime;
            ApplyDifficulty(
                Mathf.Lerp(playerMinSpeed, playerMaxSpeed, t),
                Mathf.Lerp(playerMinJump,  playerMaxJump,  t),
                Mathf.Lerp(spawnSlow,      spawnFast,      t),
                Mathf.Lerp(beltSlow,       beltFast,       t)
            );
            SetScale(t);
            return;
        }

        // PHASE 3: PEAK
        if (timer <= easyTime + rampTime + peakTime)
        {
            ApplyDifficulty(playerMaxSpeed, playerMaxJump, spawnFast, beltFast);
            SetScale(1f);
            return;
        }

        // PHASE 4: DECAY
        if (timer <= easyTime + rampTime + peakTime + decayTime)
        {
            float t = (timer - easyTime - rampTime - peakTime) / decayTime;
            ApplyDifficulty(
                Mathf.Lerp(playerMaxSpeed, 0, t),
                Mathf.Lerp(playerMaxJump,  0, t),
                Mathf.Lerp(spawnFast,      5f, t),  // essentially stop spawning
                Mathf.Lerp(beltFast,       0, t)
            );
            SetScale(1f - t);
            return;
        }

        // END
        EndGame();
    }

    void ApplyDifficulty(float speed, float jump, float spawn, float beltSpeed)
    {
        player.moveSpeed = speed;
        player.jumpForce = jump;
        spawner.spawnInterval = spawn;
        belt.beltSpeed = beltSpeed;
    }

    void SetScale(float t)
    {
        spawner.minScale = Mathf.Lerp(obstacleMinScale, obstacleMinScale * 1.2f, t);
        spawner.maxScale = Mathf.Lerp(obstacleMinScale * 1.2f, obstacleMaxScale, t);
    }

    void EndGame()
    {
        player.moveSpeed = 0;
        player.jumpForce = 0;
        spawner.spawnInterval = 999f;
        belt.beltSpeed = 0f;

        if (gameManager != null)
            gameManager.TriggerGameEnd();

        enabled = false;
    }
}
