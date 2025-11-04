using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private DifficultyHandler difficultyHandler;

    private void Update()
    {
        // Get elapsed time (assumed in seconds)
        float elapsed = difficultyHandler.ElapsedTime;

        // Convert to formatted time string
        timerText.text = FormatTime(elapsed);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return string.Format("{0}:{1:00}", minutes, seconds);
    }
}
