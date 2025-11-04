using System;
using UnityEngine;

public class PlayerStatsHandler : MonoBehaviour
{
    [Header("Core Stats")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackSpeed = 0.5f;
    [SerializeField] private float critChance = 0.1f;
    [SerializeField] private float critMultiplier = 2f;

    [Header("Game Progession")]
    [SerializeField] private int level = 1;
    [SerializeField] private float experience = 0f;
    [SerializeField] private int gems = 0;

    [Header("Level Scaling")]
    [SerializeField] private float baseExperienceToNextLevel = 100f;
    [SerializeField] private float experienceGrowthRate = 2f;

    private int maxLevel = 9; // I know this since I have 8 sprites and we start at level 1

    // Public getters for accessing stats
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float AttackRange => attackRange;
    public float AttackDamage => attackDamage;
    public float AttackSpeed => attackSpeed;
    public float CritChance => critChance;
    public float CritMultiplier => critMultiplier;
    public int Level => level;
    public int Gems => gems;

    // Events for UI updates
    public event Action<int, int> OnHealthChanged; // CurrentHealth, MaxHealth
    public event Action<float> OnExperienceChanged; // CurrentExperience as a percentage
    public event Action<int> OnLevelChanged; // Current Level
    public event Action<int> OnGemsChanged; // Current Gems

    private void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // Core methods for managing player stats
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            // Handle player death somewhere else outside of player stats (e.g., respawn, game over)
            Debug.Log("Player has died.");
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void AddExperience(float amount)
    {
        experience += amount;
        OnExperienceChanged?.Invoke(GetExperiencePercentage());
        CheckLevelUp();
    }

    public void AddGems(int amount)
    {
        gems += amount;
        OnGemsChanged?.Invoke(gems);
    }

    public void RemoveGems(int amount)
    {
        if (amount > gems)
        {
            // Trigger some UI feedback for insufficient funds
            // For now, just log a warning
            Debug.LogWarning("Not enough gold!");
            return;
        }
        gems -= amount;
        OnGemsChanged?.Invoke(gems);
    }

    public void UpgradeStat(string stat, float amount)
    {
        switch (stat.ToLower())
        {
            case "maxhealth":
                maxHealth += (int)amount;
                break;
            case "movespeed":
                moveSpeed += amount;
                break;
            case "attackrange":
                attackRange += amount;
                break;
            case "attackdamage":
                attackDamage += amount;
                break;
            default:
                Debug.LogWarning("Stat not recognized.");
                break;
        }
    }

    private float GetExperienceForNextLevel()
    {
        // Example scaling formula: baseXP * (growthRate ^ (level - 1))
        return Mathf.RoundToInt(baseExperienceToNextLevel * Mathf.Pow(experienceGrowthRate, level - 1));
    }

    private void CheckLevelUp()
    {
        // Only level when below max
        while (experience >= GetExperienceForNextLevel() && level < maxLevel)
        {
            experience -= GetExperienceForNextLevel();
            HandleLevelUp();
        }

        // Otherwise cap experience at max level
        if (level >= maxLevel)
        {
            experience = GetExperienceForNextLevel();
            OnExperienceChanged?.Invoke(1f);
        }
        else
        {
            OnExperienceChanged?.Invoke(GetExperiencePercentage());
        }
    }

    private void HandleLevelUp()
    {
        if (level >= maxLevel) return;

        level++;
        OnLevelChanged?.Invoke(level);
        Debug.Log($"Player leveled up to level {level}!");
    }

    public float GetExperiencePercentage()
    {
        return experience / GetExperienceForNextLevel();
    }
}
