using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public EntitySpawner Spawner;

    public abstract void Die();
}
