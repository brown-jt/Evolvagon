using UnityEngine;

[System.Serializable]
public struct ProjectileData
{
    public float damage;
    public bool isCritical;
    public float critMultiplier;
    public float range;

    public ProjectileData(float damage, float range, float critMultiplier, bool isCritical = false)
    {
        this.damage = damage;
        this.range = range;
        this.isCritical = isCritical;
        this.critMultiplier = critMultiplier;
    }
}
