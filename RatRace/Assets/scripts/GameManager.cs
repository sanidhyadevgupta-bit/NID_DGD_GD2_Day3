using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject GameOverPanel;
    public GameObject endingGameOverPanel; // NEW: Final game ending
    public GameObject scoreText;
    public TMPro.TextMeshProUGUI lifePhaseText;

    bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 0;
        startPanel.SetActive(true);

        if (GameOverPanel) GameOverPanel.SetActive(false);

        if (endingGameOverPanel) endingGameOverPanel.SetActive(false);

        if (scoreText) scoreText.SetActive(false);

        Debug.Log("GAME READY");
    }

    public void StartGame()
    {
        if (isGameOver) return;

        Debug.Log("START GAME PRESSED");
        Time.timeScale = 1f;
        // TURN ON STAMINA BAR
        var player = FindFirstObjectByType<PlayerController>();
        if (player != null && player.staminaBar != null)
            player.staminaBar.gameObject.SetActive(true);
        // TURN ON DIFFICULTY
        var diff = Object.FindFirstObjectByType<DifficultyManager>();
        if (diff != null)
            diff.BeginDifficulty();
        startPanel.SetActive(false);

        if (scoreText) scoreText.SetActive(true);

    }
    public string GetLifePhase(float timeSurvived)
    {
        if (timeSurvived < 30f) return "BABY";
        if (timeSurvived < 60f) return "TODDLER";
        if (timeSurvived < 90f) return "TEENAGER";
        if (timeSurvived < 120f) return "ADULT";
        if (timeSurvived < 150f) return "MIDDLE AGED";
        if (timeSurvived < 180f) return "OLD";
        return "COMPLETEION";
    }



    public void GameOver()
    {
        Time.timeScale = 0f;

        float timeLived = Time.timeSinceLevelLoad;
        string phase = GetLifePhase(timeLived);

        Debug.Log("PHASE = " + phase + " | Time = " + timeLived);

        if (lifePhaseText != null)
            lifePhaseText.text = "IN THIS RAT RACE YOU SURVIVED TILL YOU WERE " + phase;
        else
            Debug.LogError("LIFEPHASE TEXT NOT ASSIGNED!");

        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);
        else
            Debug.LogError("GAME OVER PANEL NOT ASSIGNED!");
    }





    // NEW: Called by DifficultyManager at the end of the game arc
    public void TriggerGameEnd()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GAME OVER (END OF LIFE)");
        Time.timeScale = 0f;

        if (endingGameOverPanel) endingGameOverPanel.SetActive(true);
        else Debug.LogWarning("ENDING PANEL NOT ASSIGNED");
    }

    public void Restart()
    {
        Debug.Log("RESTART");
        Time.timeScale = 1f;
    
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
