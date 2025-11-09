using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenuHandler : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicValue;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxValue;

    [Header("Test SFX Helpers")]
    [SerializeField] private SliderRelease sfxSliderRelease;
    [SerializeField] private AudioClip testSfxClip;

    private void Start()
    {
        // Set sliders
        musicSlider.value = AudioManager.Instance.MusicSource.volume * 100f;
        sfxSlider.value = AudioManager.Instance.SFXSource.volume * 100f;

        musicValue.text = (AudioManager.Instance.MusicSource.volume * 100f).ToString();
        sfxValue.text = (AudioManager.Instance.SFXSource.volume * 100f).ToString();

        // Add listeners to sliders
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);

        // Special helper to check on release of SFX slider to play test clip
        sfxSliderRelease.OnRelease = (value) =>
        {
            if (testSfxClip != null)
                AudioManager.Instance.PlaySFX(testSfxClip);
        };
    }

    private void OnMusicSliderChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value / 100f);
        musicValue.text = value.ToString();
    }

    private void OnSFXSliderChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value / 100f);
        sfxValue.text = value.ToString();
    }
}
