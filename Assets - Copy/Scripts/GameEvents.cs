using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }

    // Events
    public event Action OnChestCollected;
    public event Action OnPlayerDied;
    public event Action OnEnemyKilled;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void OnEnable()
    {
        Instance = this;
    }

    public void ChestCollected() => OnChestCollected?.Invoke();
    public void PlayerDied() => OnPlayerDied?.Invoke();
    public void EnemyKilled() => OnEnemyKilled?.Invoke();
}