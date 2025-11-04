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
            nextFireTime = Time.time + playerStats.AttackSpeed;
        }
    }

    private void OnFireStarted(InputAction.CallbackContext context) => isFiring = true;
    private void OnFireCanceled(InputAction.CallbackContext context) => isFiring = false;

    private void FireProjectile()
    {
        if (!projectilePrefab || !aimCursor) return;

        // Get direction toward AimCursor
        Vector2 direction = (aimCursor.position - transform.position).normalized;

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
                isCritical: UnityEngine.Random.value < playerStats.CritChance,
                critMultiplier: playerStats.CritMultiplier
            );

            projectile.projectileData = data;
        }

        // Apply velocity
        Rigidbody2D rb = projectileObject.GetComponent<Rigidbody2D>();
        if (rb)
            rb.linearVelocity = direction * projectileSpeed;

        // Play SFX
        AudioManager.Instance.PlaySFX(gunSound);
    }
}
