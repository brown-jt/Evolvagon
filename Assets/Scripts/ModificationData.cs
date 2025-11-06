using UnityEngine;

[System.Serializable]
public class ModificationData
{
    public string modificationName;
    public string description;
    public Sprite icon;
    public System.Action ApplyModification;

    public ModificationData(string modificationName, string description, Sprite icon, System.Action applyModification)
    {
        this.modificationName = modificationName;
        this.description = description;
        this.icon = icon;
        this.ApplyModification = applyModification;
    }
}
