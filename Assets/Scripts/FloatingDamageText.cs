using TMPro;
using UnityEngine;

public class FloatingDamageText : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private Vector2 randomDirectionRange = new Vector2(-1f, 1f);

    [Header("References")]
    [SerializeField] private TextMeshProUGUI damage;

    private float timer;
    private Color originalColour = Color.white;
    private Color critColour = Color.red;
    private Vector3 direction;
    private bool isCritical = false;

    private void Start()
    {
        damage = GetComponent<TextMeshProUGUI>();
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

        if (isCritical)
        {
            damage.color = new Color(critColour.r, critColour.g, critColour.b, t);
            damage.fontSize = Random.Range(40, 50);
        }
        else
        {
            damage.color = new Color(originalColour.r, originalColour.g, originalColour.b, t);
            damage.fontSize = Random.Range(30, 40);
        }

        if (timer <= 0)
            Destroy(gameObject);
    }

    public void SetValue(float value)
    {
        bool isWhole = Mathf.Approximately(value % 1f, 0f);

        damage.text = isWhole
            ? ((int)value).ToString()
            : value.ToString("F2");
    }
    public void SetCrit(bool value)
    {
        isCritical = value;
    }
}
