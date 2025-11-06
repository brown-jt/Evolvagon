using UnityEngine;

[System.Serializable]
public struct ProjectileData
{
    public float damage;
    public bool isCritical;
    public float critMultiplier;
    public float range;
    public Vector2 bulletDirection;
    public bool isRicochetShot;
    public bool isBouncingShot;
    public bool isExplosiveShot;
    public bool isPiercingShot;

    public ProjectileData(float damage, float range, float critMultiplier, Vector2 bulletDirection, bool isCritical = false, bool isRicochetShot = false, bool isBouncingShot = false, bool isExplosiveShot = false, bool isPiercingShot = false)
    {
        this.damage = damage;
        this.range = range;
        this.isCritical = isCritical;
        this.bulletDirection = bulletDirection;
        this.critMultiplier = critMultiplier;
        this.isRicochetShot = isRicochetShot;
        this.isBouncingShot = isBouncingShot;
        this.isExplosiveShot = isExplosiveShot;
        this.isPiercingShot= isPiercingShot;
    }
}
