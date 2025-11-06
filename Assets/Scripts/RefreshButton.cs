using TMPro;
using UnityEngine;

public class RefreshButton : MonoBehaviour
{
    [Header("External References")]
    [SerializeField] private PlayerStatsHandler playerStats;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private ModificationHandler modificationHandler;

    [Header("Button References")]
    [SerializeField] private TextMeshProUGUI required;

    int startingValue = 2;
    int requiredValue;

    private void Start()
    {
        requiredValue = startingValue;
        required.text = requiredValue.ToString();
    }

    public void RefreshOptions()
    {
        if (playerStats.Gems >= requiredValue)
        {
            playerStats.RemoveGems(requiredValue);
            upgradeManager.GenerateUpgradeOptions();
            CalculateNextValue();
        }
        required.text = requiredValue.ToString();
    }

    public void RefreshModifications()
    {
        if (playerStats.Gems >= requiredValue)
        {
            playerStats.RemoveGems(requiredValue);
            modificationHandler.GenerateModificationOptions();
            CalculateNextValue();
        }
        required.text = requiredValue.ToString();
    }

    private void CalculateNextValue()
    {
        // Maybe make this scale but for now +1 is reasonable
        requiredValue = requiredValue+1;
    }
}
