using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Handle collision with enemy e.g., apply damage here if needed
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("GameBounds"))
        {
            Destroy(gameObject);
        }
    }
}
