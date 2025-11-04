using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class OrbitWeapon : MonoBehaviour
{
    [Header("Orbit Settings")]
    [SerializeField] private float orbitSpeed = 90f;
    [SerializeField] private float orbitRadius = 2.0f;

    [Header("Weapon Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private CircleCollider2D detectionCollider;
    [SerializeField] private AudioClip gunSound;

    private Transform playerTransform;
    private float nextFireTime = 0f;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Start()
    {
        // Set up the orbit position
        playerTransform = transform.parent;
        transform.position = playerTransform.position + new Vector3(orbitRadius, 0f, 0f);

        // Set up the detection collider
        detectionCollider.radius = detectionRadius;
        detectionCollider.isTrigger = true;

        nextFireTime = Time.time;
    }

    private void Update()
    {
        Orbit();

        GameObject closestEnemy = GetClosestEnemy();
        if (closestEnemy != null && Time.time >= nextFireTime)
        {
            FireProjectileTowards(closestEnemy);
            nextFireTime = Time.time + fireRate;
        }
    }
    
    private void Orbit()
    {
        transform.RotateAround(playerTransform.position, Vector3.forward, orbitSpeed * Time.deltaTime);
    }

    private void FireProjectileTowards(GameObject target)
    {
        if (projectilePrefab == null) return;

        // Get direction toward target
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // Instantiate projectile
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Apply projectile data
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            ProjectileData data = new ProjectileData
            (
                damage: 1f,
                range: 1f,
                isCritical: Random.value < 0.2f,
                critMultiplier: 2f
            );

            projectile.projectileData = data;
        }


        // Alter colour of projectile
        SpriteRenderer sr = projectileObject.GetComponentInChildren<SpriteRenderer>();
        if (sr)
        {
            ColorUtility.TryParseHtmlString("#e3ce20", out Color newColour);
            sr.color = newColour;
        }

        // Apply velocity to projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb)
            rb.linearVelocity = direction * projectileSpeed;

        // Play SFX
        AudioManager.Instance.PlaySFX(gunSound);
    }

    private GameObject GetClosestEnemy()
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null) continue; // in case enemy was destroyed

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !enemiesInRange.Contains(collision.gameObject))
        {
            enemiesInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(collision.gameObject);
        }
    }
}
