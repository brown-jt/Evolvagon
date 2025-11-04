using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float lifetime = 1f;
    public ProjectileData projectileData;

    void Start()
    {
        Destroy(gameObject, lifetime);
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
            Destroy(gameObject);
        }
        else if (other.CompareTag("GameBounds"))
        {
            Destroy(gameObject);
        }
    }
}
