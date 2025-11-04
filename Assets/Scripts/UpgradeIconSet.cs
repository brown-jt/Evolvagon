using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeIconSet", menuName = "Upgrades/Icon Set")]
public class UpgradeIconSet : ScriptableObject
{
    public Sprite movementSpeedIcon;
    public Sprite damageIcon;
    public Sprite attackSpeedIcon;
    public Sprite attackRangeIcon;
    public Sprite critChanceIcon;
    public Sprite critDamageIcon;
}
