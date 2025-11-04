using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance;

    private int gameScore;
    public int GameScore => gameScore;

    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        gameScore += amount;
        OnScoreChanged?.Invoke(gameScore);
    }
}
