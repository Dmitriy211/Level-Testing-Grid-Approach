using System;
using UnityEngine;

[Serializable]
public class PlayerCharacteristics
{
    [Range(0, 1)] public float Killer;
    [Range(0, 1)] public float Achiever;
    [Range(0, 1)] public float Socializer;
    [Range(0, 1)] public float Explorer;

    public PlayerCharacteristics(float achiever, float explorer, float socializer, float killer)
    {
        Killer = killer;
        Achiever = achiever;
        Socializer = socializer;
        Explorer = explorer;
    }
}
