using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private Vector2 randomDirectionRange = new Vector2(-1f, 1f);

    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;

    private float timer;
    private Color originalColour = Color.cyan;
    private Vector3 direction;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timer = lifetime;

        // Random floating direction
        direction = new Vector3
        (
            Random.Range(randomDirectionRange.x, randomDirectionRange.y),
            Random.Range(0.5f, 1.2f),   // always goes upward somewhat
            0
        );
    }

    private void Update()
    {
        // Move text
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Fade out
        timer -= Time.deltaTime;
        float t = timer / lifetime;

        text.color = new Color(originalColour.r, originalColour.g, originalColour.b, t);

        if (timer <= 0)
            Destroy(gameObject);
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }
}
