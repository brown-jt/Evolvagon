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
        switch (stat)
        {
            case "Movement Speed":
                moveSpeed.text = val.ToString("F2"); break;
            case "Damage":
                damage.text = val.ToString("F2"); break;
            case "Attack Speed":
                attackSpeed.text = val.ToString("F2"); break;
            case "Attack Range":
                attackRange.text = val.ToString("F2"); break;
            case "Crit Chance":
                critChance.text = val.ToString("F2"); break;
            case "Crit Damage":
                critDamage.text = val.ToString("F2"); break;
        }
    }
    private void UpdateStat(string stat, int val)
    {
        switch (stat)
        {
            case "Movement Speed":
                moveSpeed.text = val.ToString(); break;
            case "Damage":
                damage.text = val.ToString(); break;
            case "Attack Speed":
                attackSpeed.text = val.ToString(); break;
            case "Attack Range":
                attackRange.text = val.ToString(); break;
            case "Crit Chance":
                critChance.text = val.ToString(); break;
            case "Crit Damage":
                critDamage.text = val.ToString(); break;
        }
    }
}
