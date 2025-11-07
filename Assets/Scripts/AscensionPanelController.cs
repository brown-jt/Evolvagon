using UnityEngine;

public class AscensionPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ascensionPanel;
    [SerializeField] private AscensionManager AscensionManager;

    private bool isVisible = false;

    public bool IsVisible => isVisible;

    private void Start()
    {
        HidePanel();
    }

    public void ShowPanel()
    {
        isVisible = true;
        ascensionPanel.SetActive(true);
        AscensionManager.GenerateAscensionOptions();

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HidePanel()
    {
        isVisible = false;
        ascensionPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
