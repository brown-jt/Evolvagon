using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManualGun : MonoBehaviour
{
    [Header("Manual Gun Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private Transform aimCursor;

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
            nextFireTime = Time.time + fireRate;
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
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Apply velocity
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb)
            rb.linearVelocity = direction * projectileSpeed;

        // Rotate projectile to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
