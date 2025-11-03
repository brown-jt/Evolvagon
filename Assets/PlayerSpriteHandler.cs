using UnityEngine;

public class PlayerSpriteHandler : MonoBehaviour
{
    [Header("Player Sprite Settings")]
    [SerializeField] private Sprite[] playerSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // TODO - Change sprite based on player level
    private void Start()
    {
        spriteRenderer.sprite = playerSprites[0];
    }
}
