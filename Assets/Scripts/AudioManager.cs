using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Files")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public AudioSource MusicSource => musicSource;
    public AudioSource SFXSource => sfxSource;

    // Singleton
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.resource = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void SetBGMusicPitch(float pitch)
    {
        musicSource.pitch = pitch;
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
