using System;
using System.Collections;
using System.Collections.Generic;
using Plugins;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public UnityEvent Won;
    public UnityEvent Lost;

    private List<EntitySpawner> _spawners = new List<EntitySpawner>();

    public bool TryAddSpawner(EntitySpawner spawner)
    {
        if (_spawners.Contains(spawner)) return false;
        _spawners.Add(spawner);
        return true;
    }
    
    public bool TryRemoveSpawner(EntitySpawner spawner)
    {
        if (!_spawners.Contains(spawner)) return false;
        _spawners.Remove(spawner);
        return true;
    }
    
    public void Lose()
    {
        Lost?.Invoke();
        Debug.Log("Lose!");
    }
    
    public void Win()
    {
        Won?.Invoke();
        Debug.Log("Win!");
    }

    [EditorButton()]
    public void StartGame()
    {
        foreach (EntitySpawner spawner in _spawners)
        {
            spawner.Spawn();
        }
    }

    [EditorButton()]
    public void EndGame()
    {
        foreach (EntitySpawner spawner in _spawners)
        {
            spawner.Despawn();
        }
    }

    public void ResetGame()
    {
        foreach (EntitySpawner spawner in _spawners)
        {
            spawner.ResetEntity();
        }
    }
}
