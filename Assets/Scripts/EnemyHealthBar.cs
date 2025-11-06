using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Enemy Health Bar Settings")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Enemy enemy;

    private void Start()
    {
        if (enemy == null)
        {
            Debug.LogError("EnemyHealthBar requires an enemy to be set!");
            return;
        }

        // Subscribe to player stat events
        enemy.OnHealthChanged += UpdateHealthBar;

        // Initialize the UI immediately
        UpdateHealthBar(enemy.CurrentHealth, enemy.MaxHealth);
    }
    private void OnDestroy()
    {
        if (enemy != null)
        {
            enemy.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void UpdateHealthBar(float current, float max)
    {
        healthBar.value = current;
        healthBar.maxValue = max;
    }
}
