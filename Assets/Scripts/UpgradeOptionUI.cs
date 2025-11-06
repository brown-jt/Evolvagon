using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeOptionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI rarity;
    [SerializeField] private Image rarityBackground;
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private TextMeshProUGUI oldValue;
    [SerializeField] private TextMeshProUGUI newValue;
    [SerializeField] private Image icon;

    private UpgradePanelController upgradePanelController;
    private UpgradeData upgradeData;

    public void Setup(UpgradeData data)
    {
        upgradeData = data;
        upgradePanelController = FindFirstObjectByType<UpgradePanelController>();

        rarity.text = data.rarity.ToString();
        rarityBackground.color = GetRarityColour(data.rarity);
        upgradeName.text = data.upgradeName;
        upgradeDescription.text = data.description;
        oldValue.text = FormatStat(data.upgradeName, data.oldValue);
        newValue.text = FormatStat(data.upgradeName, data.newValue);
        icon.sprite = data.icon;
    }

    private string FormatStat(string statName, float val)
    {
        switch (statName)
        {
            case "Movement Speed":
                return val.ToString("F2");
            case "Damage":
                return val.ToString("F2");
            case "Attack Speed":
                return $"{val:F2}/s";
            case "Attack Range":
                return val.ToString("F2");
            case "Crit Chance":
                return $"{val * 100}%";
            case "Crit Damage":
                return $"{val:F2}x";
            default:
                return val.ToString("F2");
        }
    }

    private Color GetRarityColour(UpgradeRarity rarity)
    {
        return rarity switch
        {
            UpgradeRarity.Common => new Color(0.4f, 0.4f, 0.4f),
            UpgradeRarity.Uncommon => Color.green,
            UpgradeRarity.Rare => Color.blue,
            UpgradeRarity.Epic => new Color(0.6f, 0f, 1f),
            UpgradeRarity.Legendary => new Color(1f, 0.5f, 0f),
            UpgradeRarity.Mythical => Color.red,
            _ => new Color(0.4f, 0.4f, 0.4f)
        };
    }

    public void OnClick()
    {
        upgradeData.ApplyUpgrade?.Invoke();

        if (upgradePanelController != null)
            upgradePanelController.HidePanel();
    }
}
