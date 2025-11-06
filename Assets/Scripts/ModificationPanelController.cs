using UnityEngine;

public class ModificationPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject modificationPanel;
    [SerializeField] private ModificationHandler modificationHandler;

    private void Start()
    {
        modificationPanel.SetActive(false);
    }

    public void ShowPanel()
    {
        modificationPanel.SetActive(true);
        modificationHandler.GenerateModificationOptions();

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HidePanel()
    {
        modificationPanel.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }
}
