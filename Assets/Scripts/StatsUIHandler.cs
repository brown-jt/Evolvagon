using System.Data;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StatsUIHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStatsHandler playerStats;
    [SerializeField] private TextMeshProUGUI moveSpeed;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI attackSpeed;
    [SerializeField] private TextMeshProUGUI attackRange;
    [SerializeField] private TextMeshProUGUI critChance;
    [SerializeField] private TextMeshProUGUI critDamage;

    private void Start()
    {
        // Subcsribe
        playerStats.OnFloatStatChanged += UpdateStat;
        playerStats.OnIntStatChanged += UpdateStat;

        // Initialize immediately
        UpdateStat("Movement Speed", playerStats.MoveSpeed);
        UpdateStat("Damage", playerStats.AttackDamage);
        UpdateStat("Attack Speed", playerStats.AttackSpeed);
        UpdateStat("Attack Range", playerStats.AttackRange);
        UpdateStat("Crit Chance", playerStats.CritChance);
        UpdateStat("Crit Damage", playerStats.CritMultiplier);
    }

    private void OnDestroy()
    {
        playerStats.OnFloatStatChanged -= UpdateStat;
        playerStats.OnIntStatChanged -= UpdateStat;
    }

    private void UpdateStat(string stat, float val)
    {
        bool isWhole = Mathf.Approximately(val % 1f, 0f);
        string formatted = isWhole ? ((int)val).ToString() : val.ToString("F2");

        switch (stat)
        {
            case "Movement Speed":
                moveSpeed.text = formatted; break;
            case "Damage":
                damage.text = formatted; break;
            case "Attack Speed":
                attackSpeed.text = $"{formatted}/s"; break;
            case "Attack Range":
                attackRange.text = formatted; break;
            case "Crit Chance":
                critChance.text = $"{(isWhole ? ((int)(val * 100)).ToString() : (val * 100).ToString("F2"))}%"; break;
            case "Crit Damage":
                critDamage.text = $"{formatted}x"; break;
        }
    }
    private void UpdateStat(string stat, int val)
    {
        bool isWhole = Mathf.Approximately(val % 1f, 0f);
        string formatted = isWhole ? ((int)val).ToString() : val.ToString("F2");

        switch (stat)
        {
            case "Movement Speed":
                moveSpeed.text = formatted; break;
            case "Damage":
                damage.text = formatted; break;
            case "Attack Speed":
                attackSpeed.text = $"{formatted}/s"; break;
            case "Attack Range":
                attackRange.text = formatted; break;
            case "Crit Chance":
                critChance.text = $"{(isWhole ? ((int)(val * 100)).ToString() : (val * 100).ToString("F2"))}%"; break;
            case "Crit Damage":
                critDamage.text = $"{formatted}x"; break;
        }
    }
}
