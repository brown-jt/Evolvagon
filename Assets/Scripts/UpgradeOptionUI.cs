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
        oldValue.text = data.oldValue.ToString("F2");
        newValue.text = data.newValue.ToString("F2");
        icon.sprite = data.icon;
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
