using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {
        // Press ESC to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;            // Freeze game
        isPaused = true;
        AudioListener.pause = true;

    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;            // Resume game
        isPaused = false;
        AudioListener.pause = false;

    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
        AudioListener.pause = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // In editor only
#endif
    }
}
