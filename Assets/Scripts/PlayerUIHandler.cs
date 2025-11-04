using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    [Header("Player UI References")]
    [SerializeField] private PlayerStatsHandler playerStats;
    [Space(10)]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;
    [Space(10)]
    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TextMeshProUGUI experienceText;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI goldText;

    private void Start()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStatsHandler requires a reference to be set!");
            return;
        }

        // Subscribe to player stat events
        playerStats.OnHealthChanged += UpdateHealthBar;
        playerStats.OnExperienceChanged += UpdateExperienceBar;
        playerStats.OnLevelChanged += UpdateExperienceBarLevel;
        playerStats.OnGoldChanged += UpdateGold;

        // Initialize the UI immediately
        UpdateHealthBar(playerStats.CurrentHealth, playerStats.MaxHealth);
        UpdateExperienceBar(playerStats.GetExperiencePercentage());
        UpdateExperienceBarLevel(playerStats.Level);
        UpdateGold(playerStats.Gold);
    }

    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged -= UpdateHealthBar;
            playerStats.OnExperienceChanged -= UpdateExperienceBar;
            playerStats.OnLevelChanged -= UpdateExperienceBarLevel;
            playerStats.OnGoldChanged -= UpdateGold;
        }
    }

    // Update methods to be called through event subscriptions
    private void UpdateHealthBar(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
        healthText.text = $"{current} / {max}";
    }

    private void UpdateExperienceBar(float percentage)
    {
        experienceSlider.value = percentage;
    }

    private void UpdateExperienceBarLevel(int level)
    {
        experienceText.text = $"LV.{level}";
    }

    private void UpdateGold(int amount)
    {
        // TODO
    }
}
