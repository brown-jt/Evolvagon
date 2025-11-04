using UnityEngine;

public class DifficultyBarSlider : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DifficultyHandler difficultyHandler;
    [SerializeField] private RectTransform difficultyContainer;

    [Header("Bar Settings")]
    [SerializeField] private float segmentWidth = 300f;
    [SerializeField] private float smoothSpeed = 5f;

    private int totalDifficulties;
    private Vector2 initialPosition;

    void Awake()
    {
        // Get total number of difficulties from enum
        totalDifficulties = System.Enum.GetNames(typeof(Difficulty)).Length;

        if (difficultyContainer != null)
        {
            // Save the initial editor-set position
            initialPosition = difficultyContainer.anchoredPosition;
        }
    }

    void Update()
    {
        if (difficultyHandler == null || difficultyContainer == null) return;

        // Get normalized progress (0 to 1)
        float progress = difficultyHandler.GetScrollProgress();

        // Total width the container can scroll
        float totalWidth = segmentWidth * totalDifficulties;

        // Target position based on progress
        Vector2 targetPos = new Vector2(initialPosition.x - progress * totalWidth, initialPosition.y);

        // Smoothly move container from its current position to the target
        difficultyContainer.anchoredPosition = Vector2.Lerp(difficultyContainer.anchoredPosition, targetPos, Time.deltaTime * smoothSpeed);
    }
}
