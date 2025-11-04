using UnityEngine;

public class UpgradePanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private PlayerStatsHandler playerStats;

    private void Start()
    {
        HidePanel();

        // Subscribing to player level up action
        if (playerStats != null)
            playerStats.OnLevelChanged += HandleLevelChanged;
    }
    private void OnDestroy()
    {
        if (playerStats != null)
            playerStats.OnLevelChanged -= HandleLevelChanged;
    }

    public void ShowPanel()
    {
        upgradePanel.SetActive(true);
        upgradeManager.GenerateUpgradeOptions();

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HidePanel()
    {
        upgradePanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void HandleLevelChanged(int _level)
    {
        ShowPanel();
    }
}
