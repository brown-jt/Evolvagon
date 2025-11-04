using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Files")]
    [SerializeField] private AudioSource BGMusicSource;
    [SerializeField] private AudioSource SFXSource;

    // Singleton
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void SetBGMusicPitch(float pitch)
    {
        BGMusicSource.pitch = pitch;
    }
}
