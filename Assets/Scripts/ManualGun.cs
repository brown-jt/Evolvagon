using UnityEngine;
using UnityEngine.InputSystem;

public class ManualGun : MonoBehaviour
{
    [Header("Player Stats Reference")]
    [SerializeField] private PlayerStatsHandler playerStats;

    [Header("Manual Gun Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Transform aimCursor;
    [SerializeField] private AudioClip gunSound;

    [Header("Input")]
    [SerializeField] private InputActionReference fireAction;

    // Weapon modifiers
    private bool tripleShotEnabled = false;
    private bool explosiveShotEnabled = false;
    private bool piercingShotEnabled = false;
    private bool bouncingShotEnabled = false;
    private bool mirrorShotEnabled = false;
    private bool ricochetShotEnabled = false;
    private Color projectileColour = Color.white;

    private float nextFireTime = 0f;
    private bool isFiring = false;

    private void OnEnable()
    {
        if (fireAction != null)
        {
            fireAction.action.started += OnFireStarted;
            fireAction.action.canceled += OnFireCanceled;
            fireAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (fireAction != null)
        {
            fireAction.action.started -= OnFireStarted;
            fireAction.action.canceled -= OnFireCanceled;
            fireAction.action.Disable();
        }
    }

    private void Update()
    {
        if (isFiring && Time.time >= nextFireTime)
        {
            FireProjectile();
            float fireInterval = 1f / Mathf.Max(playerStats.AttackSpeed, 0.01f);
            nextFireTime = Time.time + fireInterval;
        }
    }

    private void OnFireStarted(InputAction.CallbackContext context) => isFiring = true;
    private void OnFireCanceled(InputAction.CallbackContext context) => isFiring = false;

    private void FireProjectile()
    {
        if (!projectilePrefab || !aimCursor) return;

        // Get direction toward AimCursor
        Vector2 direction = (aimCursor.position - transform.position).normalized;

        SpawnProjectile(direction);
        SpawnExtraProjectiles(direction);

        // Play SFX
        AudioManager.Instance.PlaySFX(gunSound);
    }

    public void EnableModification(string mod)
    {
        switch (mod)
        {
            case "Triple Shot":
                tripleShotEnabled = true; break;
            case "Explosive Shot":
                explosiveShotEnabled = true;
                break;
            case "Piercing Shot":
                piercingShotEnabled = true; break;
            case "Bouncing Shot":
                bouncingShotEnabled = true;
                break;
            case "Recoil Shot":
                mirrorShotEnabled = true;
                break;
            case "Ricochet Shot":
                ricochetShotEnabled = true;
                break;
        }
    }

    public bool HasModification(ModificationType type)
    {
        switch (type)
        {
            case ModificationType.TripleShot:
                return tripleShotEnabled;
            case ModificationType.ExplosiveShot:
                return explosiveShotEnabled;
            case ModificationType.PiercingShot:
                return piercingShotEnabled;
            case ModificationType.BouncingShot:
                return bouncingShotEnabled;
            case ModificationType.MirrorShot:
                return mirrorShotEnabled;
            case ModificationType.RicochetShot:
                return ricochetShotEnabled;
            default:
                return false;
        }
    }

    private void SpawnProjectile(Vector2 direction)
    {
        // Instantiate projectile
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Apply projectile data
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            ProjectileData data = new ProjectileData
            (
                damage: playerStats.AttackDamage,
                range: playerStats.AttackRange,
                bulletDirection: direction,
                isCritical: UnityEngine.Random.value < playerStats.CritChance,
                critMultiplier: playerStats.CritMultiplier,
                isRicochetShot: ricochetShotEnabled,
                isBouncingShot: bouncingShotEnabled,
                isExplosiveShot: explosiveShotEnabled,
                isPiercingShot: piercingShotEnabled
            );

            projectile.projectileData = data;
        }

        // Apply colour
        projectileObject.GetComponentInChildren<SpriteRenderer>().color = GetProjectileColour();

        // Apply velocity
        Rigidbody2D rb = projectileObject.GetComponent<Rigidbody2D>();
        if (rb)
            rb.linearVelocity = direction * projectileSpeed;
    }

    private void SpawnExtraProjectiles(Vector2 aimDirection)
    {
        if (tripleShotEnabled)
        {
            float angleIncrease = 15f;    
            
            // Rotated +15
            Vector2 rightDir = Quaternion.Euler(0, 0, angleIncrease) * aimDirection;
            SpawnProjectile(rightDir);

            // Rotated −15
            Vector2 leftDir = Quaternion.Euler(0, 0, -angleIncrease) * aimDirection;
            SpawnProjectile(leftDir);
        }

        if (mirrorShotEnabled)
        {
            SpawnProjectile(-aimDirection);
        }
    }

    private Color GetProjectileColour()
    {
        if (explosiveShotEnabled)
            return Color.red;

        if (piercingShotEnabled && bouncingShotEnabled)
            return new Color(100, 150, 0);

        if (ricochetShotEnabled && bouncingShotEnabled)
            return new Color(0, 255, 255);

        if (piercingShotEnabled)
            return Color.yellow;

        if (bouncingShotEnabled)
            return Color.blue;

        if (ricochetShotEnabled)
            return Color.green;
      
        return Color.white;
    }
}
