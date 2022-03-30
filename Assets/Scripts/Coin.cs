using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Entity
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStatistics playerStatistics))
        {
            playerStatistics.IncrementCoins();
            Die();
        }
    }

    public override void Die()
    {
        LevelBlock block = Spawner.GetComponentInParent<LevelBlock>();
        GetComponentInParent<LevelBuilder>().ChangeCurrentContent(block, BlockType.Floor);
        
        gameObject.SetActive(false);
    }
}
