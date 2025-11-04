using UnityEngine;

public class PlayerSpriteHandler : MonoBehaviour
{
    [Header("Player Stats Reference")]
    [SerializeField] private PlayerStatsHandler playerStats;

    [Header("Player Sprite Settings")]
    [SerializeField] private Sprite[] playerSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // TODO - Change sprite based on player level
    private void Start()
    {
        // Subscribe to player stat events
        playerStats.OnLevelChanged += UpdatePlayerSprite;

        // Initialize straight away
        UpdatePlayerSprite(playerStats.Level);
    }
    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnLevelChanged -= UpdatePlayerSprite;
        }
    }

    private void UpdatePlayerSprite(int level)
    {
        // Since level starts at 1 and index of sprites starts at 0 ensure we minus 1.
        spriteRenderer.sprite = playerSprites[level-1];
    }
}
