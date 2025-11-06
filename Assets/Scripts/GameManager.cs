using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance;

    public AudioClip gameMusic;

    private int gameScore;
    public int GameScore => gameScore;

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AudioManager.Instance.PlayMusic(gameMusic);
    }

    public void AddScore(int amount)
    {
        gameScore += amount;
        OnScoreChanged?.Invoke(gameScore);
    }
}
