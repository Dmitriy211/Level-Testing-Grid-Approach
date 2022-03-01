using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeProjectile : MonoBehaviour
{
    [SerializeField] private float _timeToLive;

    private void Update()
    {
        _timeToLive -= Time.deltaTime;
        if (_timeToLive < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            entity.Die();
        }
    }
}
