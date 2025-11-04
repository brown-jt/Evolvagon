using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject upgradeOptionPrefab;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private PlayerStatsHandler playerStats;
    [SerializeField] private UpgradeIconSet upgradeIconSet;

    private int numberOfOptions = 3;

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
        string description = "";
        Sprite icon = null;

        switch (type)
        {
            case UpgradeType.MovementSpeed:
                oldVal = playerStats.MoveSpeed;
                newVal = playerStats.MoveSpeed * rarityMultiplier;
                description = "Gotta go fast!";
                icon = upgradeIconSet.movementSpeedIcon;
                return new UpgradeData(rarity, "Movement Speed", description, oldVal, newVal, icon, 
                    () => Debug.Log("Upgrade player movement speed"));

            case UpgradeType.Damage:
                oldVal = playerStats.AttackDamage;
                newVal = playerStats.AttackDamage * rarityMultiplier;
                description = "Big number = Big happy";
                icon = upgradeIconSet.damageIcon;
                return new UpgradeData(rarity, "Damage", description, oldVal, newVal, icon,
                    () => Debug.Log("Upgrade player damage"));

            case UpgradeType.AttackSpeed:
                oldVal = playerStats.AttackSpeed;
                newVal = playerStats.AttackSpeed * rarityMultiplier;
                description = "pew pew pew pew pew pew pew";
                icon = upgradeIconSet.attackSpeedIcon;
                return new UpgradeData(rarity, "Attack Speed", description, oldVal, newVal, icon,
                    () => Debug.Log("Upgrade player attack speed"));

            case UpgradeType.AttackRange:
                oldVal = playerStats.AttackRange;
                newVal = playerStats.AttackRange * rarityMultiplier;
                description = "Eye see everything";
                icon = upgradeIconSet.attackRangeIcon;
                return new UpgradeData(rarity, "Attack Range", description, oldVal, newVal, icon,
                    () => Debug.Log("Upgrade player attack range"));

            case UpgradeType.CriticalChance:
                oldVal = playerStats.CritChance;
                newVal = playerStats.CritChance * rarityMultiplier;
                description = "Want more big numbers?";
                icon = upgradeIconSet.critChanceIcon;
                return new UpgradeData(rarity, "Crit Chance", description, oldVal, newVal, icon,
                    () => Debug.Log("Upgrade player crit chance"));

            case UpgradeType.CriticalDamage:
                oldVal = playerStats.CritMultiplier;
                newVal = playerStats.CritMultiplier * rarityMultiplier;
                description = "Make big number even bigger";
                icon = upgradeIconSet.critDamageIcon;
                return new UpgradeData(rarity, "Crit Damage", description, oldVal, newVal, icon,
                    () => Debug.Log("Upgrade player crit damage"));

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
            UpgradeRarity.Epic => 1.35f,
            UpgradeRarity.Legendary => 1.50f,
            UpgradeRarity.Mythical => 1.75f,
            _ => 0.05f
        };
    }
}
