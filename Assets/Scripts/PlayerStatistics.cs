using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatistics : MonoBehaviour
{
    [field:SerializeField] public int CoinsCollected { get; private set; }
    [field:SerializeField] public int EnemiesKilled { get; private set; }
    [field:SerializeField] public float TimeSpent { get; private set; }

    public UnityEvent CoinCollected;
    public UnityEvent EnemyKilled;

    private void Update()
    {
        TimeSpent += Time.deltaTime;
    }

    public void IncrementCoins()
    {
        CoinsCollected++;
        CoinCollected?.Invoke();
    }

    public void IncrementKills()
    {
        EnemiesKilled++;
        EnemyKilled?.Invoke();
    }
}
