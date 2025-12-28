using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // <<< ADD THIS

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject leftGameOverPanel;
    public GameObject rightGameOverPanel;
    public GameObject scoreText;

    bool isGameOver = false;

    void Awake()
    {
        Instance = this;  // <<< AND ADD THIS
    }

    void Start()
    {
        Time.timeScale = 0;
        startPanel.SetActive(true);

        if (leftGameOverPanel) leftGameOverPanel.SetActive(false);
        if (rightGameOverPanel) rightGameOverPanel.SetActive(false);

        if (scoreText) scoreText.SetActive(false);

        Debug.Log("GAME READY");
    }

    public void StartGame()
    {
        if (isGameOver) return;

        Debug.Log("START GAME PRESSED");
        Time.timeScale = 1f;
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

    public void Restart()
    {
        Debug.Log("RESTART");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
