using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public override void Die()
    {
        gameObject.SetActive(false);
        GetComponentInParent<LevelManager>().Lose();
    }
}
