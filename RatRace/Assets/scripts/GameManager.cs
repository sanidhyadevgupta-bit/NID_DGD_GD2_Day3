using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject leftGameOverPanel;
    public GameObject rightGameOverPanel;
    public GameObject endingGameOverPanel; // NEW: Final game ending
    public GameObject scoreText;

    bool isGameOver = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 0;
        startPanel.SetActive(true);

        if (leftGameOverPanel) leftGameOverPanel.SetActive(false);
        if (rightGameOverPanel) rightGameOverPanel.SetActive(false);
        if (endingGameOverPanel) endingGameOverPanel.SetActive(false);

        if (scoreText) scoreText.SetActive(false);

        Debug.Log("GAME READY");
    }

    public void StartGame()
    {
        if (isGameOver) return;

        Debug.Log("START GAME PRESSED");
        Time.timeScale = 1f;

        var diff = Object.FindFirstObjectByType<DifficultyManager>();
        if (diff != null)
            diff.BeginDifficulty();
        startPanel.SetActive(false);

        if (scoreText) scoreText.SetActive(true);

    }

    public void GameOverLeft()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GAME OVER LEFT SIDE");
        Time.timeScale = 0f;

        if (leftGameOverPanel) leftGameOverPanel.SetActive(true);
    }

    public void GameOverRight()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GAME OVER RIGHT SIDE");
        Time.timeScale = 0f;

        if (rightGameOverPanel) rightGameOverPanel.SetActive(true);
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
