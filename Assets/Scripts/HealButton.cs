using UnityEngine;

public class HealButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStatsHandler playerStats;
    [SerializeField] private UpgradePanelController upgradePanelController;

    public void HealPlayer()
    {
        playerStats.Heal(playerStats.MaxHealth);
        upgradePanelController.HidePanel();
    }
}
