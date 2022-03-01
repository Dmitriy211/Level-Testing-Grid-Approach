using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerStatistics playerStatistics))
        {
            gameObject.SetActive(false);
            playerStatistics.IncrementCoins();
        }
    }
}
