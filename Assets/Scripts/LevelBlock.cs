using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    [field: SerializeField] public BlockType BlockType { get; private set; }
}
