using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject upgradeOptionPrefab;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private PlayerStatsHandler playerStats;
    [SerializeField] private UpgradeIconSet upgradeIconSet;

    private readonly int numberOfOptions = 3;

    public void GenerateUpgradeOptions()
    {
        // Clear any existing options that might be present firstly
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        // Generate new ones now
        for (int i = 0; i < numberOfOptions; i++)
        {
            UpgradeData randomUpgrade = GetRandomUpgrade();
            var option = Instantiate(upgradeOptionPrefab, optionsContainer);
            option.GetComponent<UpgradeOptionUI>().Setup(randomUpgrade);
        }
    }

    private UpgradeData GetRandomUpgrade()
    {
        // Firstly let's get a random rarity
        // TODO - Implement weighted randomness
        UpgradeRarity rarity = (UpgradeRarity)Random.Range(0, System.Enum.GetValues(typeof(UpgradeRarity)).Length);

        // Secondly let's get a random upgrade type
        // TODO - Implement a feature where you cannot get the same option shown twice
        UpgradeType type = (UpgradeType)Random.Range(0, System.Enum.GetValues(typeof(UpgradeType)).Length);

        // Build the complete UpgradeData structure
        return BuildUpgradeData(rarity, type);
    }

    private UpgradeData BuildUpgradeData(UpgradeRarity rarity, UpgradeType type)
    {
        float rarityMultiplier = GetRarityMultiplier(rarity);
        float oldVal = 0;
        float newVal = 0;
        string name = "";
        string description = "";
        Sprite icon = null;

        switch (type)
        {
            case UpgradeType.MovementSpeed:
                oldVal = playerStats.MoveSpeed;
                newVal = playerStats.MoveSpeed * rarityMultiplier;
                name = "Movement Speed";
                description = "Gotta go fast!";
                icon = upgradeIconSet.movementSpeedIcon;
                return new UpgradeData(rarity, name, description, oldVal, newVal, icon, 
                    () => playerStats.ModifyStat(name, newVal));

            case UpgradeType.Damage:
                oldVal = playerStats.AttackDamage;
                newVal = playerStats.AttackDamage * rarityMultiplier;
                name = "Damage";
                description = "Big number = Big happy";
                icon = upgradeIconSet.damageIcon;
                return new UpgradeData(rarity, name, description, oldVal, newVal, icon,
                    () => playerStats.ModifyStat(name, newVal));

            case UpgradeType.AttackSpeed:
                oldVal = playerStats.AttackSpeed;
                newVal = playerStats.AttackSpeed * rarityMultiplier;
                name = "Attack Speed";
                description = "pew pew pew pew pew pew pew";
                icon = upgradeIconSet.attackSpeedIcon;
                return new UpgradeData(rarity, name, description, oldVal, newVal, icon,
                    () => playerStats.ModifyStat(name, newVal));

            case UpgradeType.AttackRange:
                oldVal = playerStats.AttackRange;
                newVal = playerStats.AttackRange * rarityMultiplier;
                name = "Attack Range";
                description = "Eye see everything";
                icon = upgradeIconSet.attackRangeIcon;
                return new UpgradeData(rarity, name, description, oldVal, newVal, icon,
                    () => playerStats.ModifyStat(name, newVal));

            case UpgradeType.CriticalChance:
                oldVal = playerStats.CritChance;
                newVal = playerStats.CritChance * rarityMultiplier;
                name = "Crit Chance";
                description = "Want more big numbers?";
                icon = upgradeIconSet.critChanceIcon;
                return new UpgradeData(rarity, name, description, oldVal, newVal, icon,
                    () => playerStats.ModifyStat(name, newVal));

            case UpgradeType.CriticalDamage:
                oldVal = playerStats.CritMultiplier;
                newVal = playerStats.CritMultiplier * rarityMultiplier;
                name = "Crit Damage";
                description = "Make big number even bigger";
                icon = upgradeIconSet.critDamageIcon;
                return new UpgradeData(rarity, name, description, oldVal, newVal, icon,
                    () => playerStats.ModifyStat(name, newVal));

            default:
                return null;
        }
    }

    private float GetRarityMultiplier(UpgradeRarity rarity)
    {
        return rarity switch
        {
            UpgradeRarity.Common => 1.05f,
            UpgradeRarity.Uncommon => 1.10f,
            UpgradeRarity.Rare => 1.20f,
            UpgradeRarity.Epic => 1.30f,
            UpgradeRarity.Legendary => 1.40f,
            UpgradeRarity.Mythical => 1.50f,
            _ => 1.05f
        };
    }
}
