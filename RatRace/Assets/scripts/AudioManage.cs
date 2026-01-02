using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip backgroundMusic;
    public AudioClip jumpSFX;
    public AudioClip spawnSFX;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Listen for scene reloads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RestartMusic();   // 🔥 GUARANTEED RESTART POINT
    }

    public void RestartMusic()
    {
        if (musicSource == null || backgroundMusic == null) return;

        musicSource.Stop();
        musicSource.clip = backgroundMusic;
        musicSource.time = 0f;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayJump()
    {
        if (jumpSFX != null)
            sfxSource.PlayOneShot(jumpSFX);
    }

    public void PlaySpawn()
    {
        if (spawnSFX != null)
            sfxSource.PlayOneShot(spawnSFX);
    }
}
