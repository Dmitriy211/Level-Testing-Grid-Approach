using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public UnityEvent Won;
    public UnityEvent Lost;

    public void Lose()
    {
        Lost?.Invoke();
    }
    
    public void Win()
    {
        Won?.Invoke();
    }
}
