using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.5f;

    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private int currentHealth;

    [Header("Enemy Rewards")]
    [SerializeField] private float expReward = 10f;
    [SerializeField] private int gemReward = 5;
    [SerializeField] private int baseScore = 50;

    [Header("Enemy Sounds")]
    [SerializeField] private AudioClip deathSound;

    private DifficultyHandler difficultyHandler;
    private Rigidbody2D rb;
    private Transform spriteTransform;
    private bool isKnockedBack = false;
    private float difficultyScale = 1.0f;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public event Action<int, int> OnHealthChanged; // CurrentHealth, MaxHealth

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        difficultyHandler = FindFirstObjectByType<DifficultyHandler>();
    }

    private void Start()
    {
        // Calculate spawn stats from base stats * difficulty and assign
        CalculateStatsWithDifficulty(difficultyHandler.CurrentDifficulty);

        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
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

    private void CalculateStatsWithDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                difficultyScale = 1.0f; break;
            case Difficulty.MEDIUM:
                difficultyScale = 1.25f; break;
            case Difficulty.HARD:
                difficultyScale = 1.5f; break;
            case Difficulty.EXTREME:
                difficultyScale = 1.75f; break;
            case Difficulty.INSANITY:
                difficultyScale = 2f; break;
            default:
                difficultyScale = 2f; break;
        }

        // If past elapsed Insanity time (5minutes) just start mega scaling
        if (difficultyHandler.ElapsedTime > 300f)
        {
            difficultyScale += ((difficultyHandler.ElapsedTime - 300f) / 50);
            Debug.Log($"Current difficulty scale: {difficultyScale}");
        }

        ApplyDifficultyScaling();
    }

    private void ApplyDifficultyScaling()
    {
        speed = speed * difficultyScale;
        maxHealth = Mathf.RoundToInt(maxHealth * difficultyScale);
        gemReward = Mathf.RoundToInt(gemReward * difficultyScale);
        baseScore = Mathf.RoundToInt(baseScore * difficultyScale);

        // Only increase EXP gain during normal levels - Stop when mega scaling
        if (difficultyHandler.ElapsedTime < 300f)
            expReward = expReward * difficultyScale;
    }

    public void TakeDamage(ProjectileData data)
    {
        float finalDamage = data.damage;

        // If critical hit
        if (data.isCritical)
            finalDamage = finalDamage * data.critMultiplier;

        currentHealth -= Mathf.RoundToInt(finalDamage);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        AudioManager.Instance.PlaySFX(deathSound);
        PlayerStatsHandler playerStats = player.GetComponent<PlayerStatsHandler>();
        if (playerStats != null)
        {
            playerStats.AddExperience(expReward);

            // 10% chance to add gems
            if (UnityEngine.Random.value < 0.1f) 
                playerStats.AddGems(gemReward);
        }
        GameManager.Instance.AddScore(baseScore);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isKnockedBack)
        {
            // Get the player stats for easy method calling
            PlayerStatsHandler playerStats = collision.gameObject.GetComponent<PlayerStatsHandler>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(1);
            }

            // Now do the knockback effect
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
