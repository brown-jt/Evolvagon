using UnityEngine;

[System.Serializable]
public class UpgradeData
{
    public UpgradeRarity rarity;
    public string upgradeName;
    public string description;
    public float oldValue;
    public float newValue;
    public Sprite icon;
    public System.Action ApplyUpgrade;

    public UpgradeData(UpgradeRarity rarity, string upgradeName, string description, float oldValue, float newValue, Sprite icon, System.Action applyUpgrade)
    {
        this.rarity = rarity;
        this.upgradeName = upgradeName;
        this.description = description;
        this.oldValue = oldValue;
        this.newValue = newValue;
        this.icon = icon;
        this.ApplyUpgrade = applyUpgrade;
    }
}
