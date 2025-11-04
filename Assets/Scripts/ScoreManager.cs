using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("Score Manager requires a Game Manager to be set!");
            return;
        }

        // Subscribe to player stat events
        gameManager.OnScoreChanged += UpdateScore;

        // Initialize the UI immediately
        UpdateScore(0);
    }
    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnScoreChanged -= UpdateScore;
        }
    }

    private void UpdateScore(int score)
    {
        scoreText.text = $"SCORE: {score}";
    }
}
