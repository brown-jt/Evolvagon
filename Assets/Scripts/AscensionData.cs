using UnityEngine;

[System.Serializable]
public class AscensionData
{
    public string ascensionName;
    public string description;
    public Sprite icon;
    public System.Action ApplyAscension;

    public AscensionData(string ascensionName, string description, Sprite icon, System.Action applyAscension)
    {
        this.ascensionName = ascensionName;
        this.description = description;
        this.icon = icon;
        this.ApplyAscension = applyAscension;
    }
}
