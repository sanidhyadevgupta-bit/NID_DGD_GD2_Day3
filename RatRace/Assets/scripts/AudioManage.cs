using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;     // for background loop
    public AudioSource sfxSource;       // for sound effects

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
        DontDestroyOnLoad(gameObject);  // persists between scenes
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
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
