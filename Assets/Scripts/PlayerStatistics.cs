using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    [field:SerializeField] public int CoinsCollected { get; private set; }
    [field:SerializeField] public int EnemiesKilled { get; private set; }
    [field:SerializeField] public float TimeSpent { get; private set; }

    private void Update()
    {
        TimeSpent += Time.deltaTime;
    }

    public void IncrementCoins()
    {
        CoinsCollected++;
    }

    public void IncrementKills()
    {
        EnemiesKilled++;
    }
}
