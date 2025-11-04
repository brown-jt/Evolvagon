using UnityEngine;

[System.Serializable]
public struct ProjectileData
{
    public float damage;
    public bool isCritical;
    public float critMultiplier;

    public ProjectileData(float damage, float critMultiplier, bool isCritical = false)
    {
        this.damage = damage;
        this.isCritical = isCritical;
        this.critMultiplier = critMultiplier;
    }
}
