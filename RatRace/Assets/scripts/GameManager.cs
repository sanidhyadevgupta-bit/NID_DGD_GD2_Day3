using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject scoreText;

    void Start()
    {
        Time.timeScale = 0;
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        if (scoreText) scoreText.SetActive(false);

        Debug.Log("GAME READY");
    }

    public void StartGame()
    {
        Debug.Log("START GAME PRESSED");
        Time.timeScale = 1;
        startPanel.SetActive(false);
        if (scoreText) scoreText.SetActive(true);
    }

   bool isGameOver = false;

public void GameOver()
{
    if (isGameOver) return; // PREVENT MULTIPLE TRIGGERS
    isGameOver = true;

    Debug.Log("GAME OVER TRIGGERED");

    Time.timeScale = 0f;
    gameOverPanel.SetActive(true);
}


    public void Restart()
    {
        Debug.Log("RESTART");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
