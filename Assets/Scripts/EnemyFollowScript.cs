using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyFollowScript : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float knockbackForce = 50f;
    [SerializeField] private float knockbackDuration = 0.5f;

    private Rigidbody2D rb;
    private Transform spriteTransform;
    private bool isKnockedBack = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            // Get direction from enemy to player
            Vector2 direction = (player.position - transform.position).normalized;

            // Rotate sprite to face player
            Vector2 lookDir = player.position - spriteTransform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            spriteTransform.rotation = Quaternion.Euler(0, 0, angle+90);

            // Move enemy towards player
            transform.position += speed * Time.deltaTime * (Vector3)direction;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isKnockedBack)
        {
            StartCoroutine(Knockback(collision));
        }
    }
    IEnumerator Knockback(Collision2D collision)
    {
        isKnockedBack = true;

        // Calculate direction away from player
        Vector2 knockbackDir = (rb.position - (Vector2)collision.transform.position).normalized;

        float elapsed = 0f;
        while (elapsed < knockbackDuration)
        {
            // Calculate decaying speed (linear decay)
            float decayedSpeed = knockbackForce * (1f - (elapsed / knockbackDuration));

            // Move enemy using velocity
            rb.linearVelocity = knockbackDir * decayedSpeed;

            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Stop knockback and resume normal movement
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }
}
