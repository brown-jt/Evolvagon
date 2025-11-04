using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Files")]
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
}
