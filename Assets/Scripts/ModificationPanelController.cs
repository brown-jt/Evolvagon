using UnityEngine;

public class ModificationPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject modificationPanel;
    [SerializeField] private ModificationHandler modificationHandler;

    private bool isVisible = false;

    public bool IsVisible => isVisible;

    private void Start()
    {
        HidePanel();
    }

    public void ShowPanel()
    {
        isVisible = true;
        modificationPanel.SetActive(true);
        modificationHandler.GenerateModificationOptions();

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HidePanel()
    {
        isVisible = false;
        modificationPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
