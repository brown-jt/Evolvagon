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
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private CircleCollider2D detectionCollider;
    [SerializeField] private AudioClip gunSound;

    [Header("References")]
    [SerializeField] private FloatingText levelUpText;
    [SerializeField] private AscensionPanelController ascensionPanelController;

    private Transform playerTransform;
    private float nextFireTime = 0f;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    private float projectileDamage = 100f;
    private float attackSpeed = 0.5f;
    private float projectileRange = 1f;
    private float projectileCritChance = 0.1f;
    private float projectileCritMultiplier = 2f;

    private int starsCollected = 0;
    private int ascensionAmount = 1;

    private Canvas worldCanvas;

    private void Start()
    {
        // Set up the orbit position
        playerTransform = transform.parent;
        transform.position = playerTransform.position + new Vector3(orbitRadius, 0f, 0f);

        // Set up the detection collider
        detectionCollider.radius = detectionRadius;
        detectionCollider.isTrigger = true;

        nextFireTime = Time.time;

        worldCanvas = GameObject.Find("WorldCanvas").GetComponent<Canvas>();
        ascensionPanelController = FindFirstObjectByType<AscensionPanelController>();
    }

    private void Update()
    {
        Orbit();

        GameObject closestEnemy = GetClosestEnemy();
        if (closestEnemy != null && Time.time >= nextFireTime)
        {
            FireProjectileTowards(closestEnemy);
            float fireInterval = 1f / Mathf.Max(attackSpeed, 0.01f);
            nextFireTime = Time.time + fireInterval;
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
                damage: projectileDamage,
                range: projectileRange,
                isCritical: Random.value < projectileCritChance,
                bulletDirection: direction,
                critMultiplier: projectileCritMultiplier
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

    public void LevelUp()
    {
        starsCollected++;

        // Floating level up text
        if (starsCollected != ascensionAmount)
        {
            var floatingText = Instantiate(levelUpText, worldCanvas.transform);
            Vector3 offset = new Vector3(0f, 1f, 0f);
            floatingText.SetText("STAR UP!");
            floatingText.transform.position = transform.position + offset;
        }

        projectileDamage += 20f;
        attackSpeed += 0.10f;
        detectionRadius += 0.25f;
        projectileRange += 0.25f;
        projectileCritChance += 0.25f;

        // set new radius also
        detectionCollider.radius = detectionRadius;

        if (starsCollected == ascensionAmount)
        {
            ascensionPanelController.ShowPanel();

            // Floating ascension text
            var floatingText = Instantiate(levelUpText, worldCanvas.transform);
            Vector3 offset = new Vector3(0f, 1f, 0f);
            floatingText.SetText("COSMIC ASCENSION!");
            floatingText.transform.position = transform.position + offset;
        }
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
