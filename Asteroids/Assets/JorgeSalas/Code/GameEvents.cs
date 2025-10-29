using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<int> OnAsteroidDestroyed;
    public static event Action OnPlayerHit;

    public static event Action OnGameStart;
    public static event Action OnGameEnd;

    public static void AsteroidDestroyed(int scoreValue)
    {
        OnAsteroidDestroyed?.Invoke(scoreValue);
    }

    public static void PlayerHit()
    {
        OnPlayerHit?.Invoke();
    }

    public static void GameStart()
    {
        OnGameStart?.Invoke();
    }

    public static void GameEnd()
    {
        OnGameEnd?.Invoke();
    }
}
