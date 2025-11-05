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
        switch (level)
        {
            case < 3:
                spriteRenderer.sprite = playerSprites[0]; break;
            case < 5:
                spriteRenderer.sprite = playerSprites[1]; break;
            case < 7:
                spriteRenderer.sprite = playerSprites[2]; break;
            case < 9:
                spriteRenderer.sprite = playerSprites[3]; break;
            case < 11:
                spriteRenderer.sprite = playerSprites[4]; break;
            case < 13:
                spriteRenderer.sprite = playerSprites[5]; break;
            case < 15:
                spriteRenderer.sprite = playerSprites[6]; break;
            case < 17:
                spriteRenderer.sprite = playerSprites[7]; break;
            default:
                spriteRenderer.sprite = playerSprites[8]; break;
        }
    }
}
