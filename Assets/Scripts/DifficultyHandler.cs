using TMPro;
using UnityEngine;

public enum Difficulty
{
    EASY,
    MEDIUM,
    HARD,
    EXTREME,
    INSANITY
}

public class DifficultyHandler : MonoBehaviour
{
    [Header("Difficulty Settings")]
    [SerializeField] private float difficultyDuration = 60f;
    [SerializeField] private TextMeshProUGUI difficultyText;

    public Difficulty CurrentDifficulty { get; private set; }

    private float elapsedTime = 0f;
    private int totalDifficulties;

    public float ElapsedTime => elapsedTime;

    private void Awake()
    {
        totalDifficulties = System.Enum.GetValues(typeof(Difficulty)).Length;
        CurrentDifficulty = Difficulty.EASY;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        UpdateDifficulty();
    }

    private void UpdateDifficulty()
    {
        // Calculate which difficulty index we are on
        int index = Mathf.Clamp((int)(elapsedTime / difficultyDuration), 0, totalDifficulties - 1);
        CurrentDifficulty = (Difficulty)index;
        difficultyText.text = CurrentDifficulty.ToString();
        AudioManager.Instance.SetBGMusicPitch(GetMusicPitch());
    }

    // Returns a value from 0 to 1 for slider progress
    public float GetScrollProgress()
    {
        float totalDuration = difficultyDuration * totalDifficulties;
        return Mathf.Clamp01(elapsedTime / totalDuration);
    }
    
    private float GetMusicPitch()
    {
        float pitch = 1.0f;
        switch (CurrentDifficulty)
        {
            case Difficulty.INSANITY:
                pitch = 1.4f;
                break;
            case Difficulty.EXTREME:
                pitch = 1.3f;
                break;
            case Difficulty.HARD:
                pitch = 1.2f;
                break;
            case Difficulty.MEDIUM:
                pitch = 1.1f;
                break;
            case Difficulty.EASY:
                pitch = 1.0f;
                break;
        }
        return pitch;
    }
}
