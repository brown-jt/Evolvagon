using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AscensionOptionUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI ascensionName;
    [SerializeField] private TextMeshProUGUI ascensionDescription;
    [SerializeField] private Image icon;

    private AscensionPanelController ascensionPanelController;
    private AscensionData ascensionData;

    public void Setup(AscensionData data)
    {
        ascensionData = data;
        ascensionPanelController = FindFirstObjectByType<AscensionPanelController>();

        ascensionName.text = data.ascensionName;
        ascensionDescription.text = data.description;
        icon.sprite = data.icon;
    }

    public void OnClick()
    {
        ascensionData.ApplyAscension?.Invoke();

        if (ascensionPanelController != null)
            ascensionPanelController.HidePanel();
    }
}
