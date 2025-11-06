using UnityEngine;

public class EnemySpriteHandler : MonoBehaviour
{
    [Header("Enemy Sprites")]
    [SerializeField] private Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Length > 0)
        {
            int randIndex = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[randIndex];
        }
        else
        {
            Debug.LogWarning("Assign enemy sprites!");
        }
    }
}
