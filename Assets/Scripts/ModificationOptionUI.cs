using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModificationOptionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI modificationName;
    [SerializeField] private TextMeshProUGUI modificationDescription;
    [SerializeField] private Image icon;

    private ModificationPanelController modificationPanelController;
    private ModificationData modificationData;

    public void Setup(ModificationData data)
    {
        modificationData = data;
        modificationPanelController = FindFirstObjectByType<ModificationPanelController>();

        modificationName.text = data.modificationName;
        modificationDescription.text = data.description;
        icon.sprite = data.icon;
    }

    public void OnClick()
    {
        modificationData.ApplyModification?.Invoke();

        if (modificationPanelController != null)
            modificationPanelController.HidePanel();
    }
}
