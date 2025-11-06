using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private AudioClip[] explosionSFX;
    [SerializeField] private float cameraShakeDurationMin = 0.1f;
    [SerializeField] private float cameraShakeDurationMax = 0.3f;
    [SerializeField] private float cameraShakeMagnitudeMin = 0.1f;
    [SerializeField] private float cameraShakeMagnitudeMax = 0.3f;


    [Header("Animation Settings")]
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // Damage helpers
    private PlayerStatsHandler playerStats;
    private float explosionDamage;

    // Animation helpers
    private float timer;
    private int currentFrame;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = FindFirstObjectByType<PlayerStatsHandler>();
        explosionDamage = playerStats.AttackDamage;

        CreateExplosion();
    }

    private void Update()
    {
        if (frames.Length == 0) return;

        timer += Time.deltaTime;
        float frameTime = animationDuration / frames.Length;
        currentFrame = Mathf.FloorToInt(timer / frameTime);

        if (currentFrame < frames.Length)
        {
            spriteRenderer.sprite = frames[currentFrame];
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CreateExplosion()
    {
        // Play SFX
        if (explosionSFX.Length > 0)
        {
            int randIndx = Random.Range(0, explosionSFX.Length);
            AudioManager.Instance.PlaySFX(explosionSFX[randIndx]);
        }

        // Cause screen shake
        CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
        if (cameraShake != null)
        {
            float shakeDuration = Random.Range(cameraShakeDurationMin, cameraShakeDurationMax);
            float shakeMagnitude = Random.Range(cameraShakeMagnitudeMin, cameraShakeMagnitudeMax);
            cameraShake.Shake(shakeDuration, shakeMagnitude);
        }

        // Handle damage
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // Try to get an EnemyHealth component
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(explosionDamage);
                }
            }
        }
    }

    // Helper to see actual explosion area
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
