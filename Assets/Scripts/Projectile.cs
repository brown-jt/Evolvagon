using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public ProjectileData projectileData;
    [SerializeField] private GameObject explosionPrefab;

    // Piercing shot helpers
    private bool hasPierced = false;

    // Ricochet shot helpers
    private bool hasRicocheted = false;
    private GameObject lastHitEnemy;

    // Speed to re-apply to bullet after potenital direction changes
    // This is the same as in ManualGun
    private readonly float projectileSpeed = 10f;

    // Helper to aid destruction of projectile after
    private Coroutine destroyCoroutine;

    void Start()
    {
        destroyCoroutine = StartCoroutine(DestroyAfterTime(projectileData.range));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Get the enemy script to handle logic within
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(projectileData);
            }

            if (projectileData.isPiercingShot && !hasPierced)
            {
                hasPierced = true;
                if (projectileData.isExplosiveShot)
                {
                    Explode();
                }
            }
            else if (projectileData.isRicochetShot && !hasRicocheted)
            {
                lastHitEnemy = other.gameObject;
                hasRicocheted = true;
                RicochetBullet();
                if (projectileData.isExplosiveShot)
                {
                    Explode();
                }
            }
            else
            {
                if (projectileData.isExplosiveShot)
                {
                    Explode();
                }
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("GameBounds"))
        {
            if (projectileData.isBouncingShot)
            {
                BounceBullet(other);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void BounceBullet(Collider2D gameBounds)
    {
        // Reset bullet lifetime to allow for meaningfulness
        ChangeDestructionTime(projectileData.range);

        Vector2 dir = projectileData.bulletDirection;
        Bounds b = gameBounds.bounds;

        Vector2 pos = transform.position;

        // Check which side of the bounds is closest
        float leftDist = Mathf.Abs(pos.x - b.min.x);
        float rightDist = Mathf.Abs(pos.x - b.max.x);
        float bottomDist = Mathf.Abs(pos.y - b.min.y);
        float topDist = Mathf.Abs(pos.y - b.max.y);

        float min = Mathf.Min(leftDist, rightDist, bottomDist, topDist);

        // Hit left or right wall = flip X
        if (min == leftDist || min == rightDist)
            dir.x = -dir.x;

        // Hit top or bottom wall = flip Y
        if (min == topDist || min == bottomDist)
            dir.y = -dir.y;

        projectileData.bulletDirection = dir.normalized;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = projectileData.bulletDirection * projectileSpeed;
    }

    void RicochetBullet()
    {
        // Find all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // No enemies to ricochet to
        if (enemies.Length == 0)
            return;

        // Change destruction time to enable bullet to ricochet
        ChangeDestructionTime(Mathf.Infinity);

        // Find the closest enemy
        GameObject closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == lastHitEnemy)
                continue;

            Vector2 directionToEnemy = (Vector2)enemy.transform.position - currentPosition;
            float dSqrToEnemy = directionToEnemy.sqrMagnitude;
            if (dSqrToEnemy < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToEnemy;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            Vector2 direction = ((Vector2)closestEnemy.transform.position - currentPosition).normalized;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed; // Replace 'speed' with your projectile speed
            }
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void ChangeDestructionTime(float newTime)
    {
        if (destroyCoroutine != null)
        {
            StopCoroutine(destroyCoroutine);
        }
        destroyCoroutine = StartCoroutine(DestroyAfterTime(newTime));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
