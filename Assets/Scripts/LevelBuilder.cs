using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private GameObject[] _levelBlocks;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private int[] _levelContent;

    private GameObject[] _levelObjects;

    private void OnValidate()
    {
        if (_levelContent.Length != _gridSize.x * _gridSize.y)
        {
            int[] tmp = _levelContent;
            _levelContent = new int[_gridSize.x * _gridSize.y];

            for (int i = 0; i < _levelContent.Length; i++)
                _levelContent[i] = tmp[i];
        }
    }

    private void Awake()
    {
        _levelObjects = new GameObject[_levelContent.Length];
    }

    private void Start()
    {
        BuildLevel();
    }

    private void BuildLevel()
    {
        for (int i = 0; i < _gridSize.x; i++)
        {
            for (int j = 0; j < _gridSize.y; j++)
            {
                _levelObjects[i*_gridSize.x + j] = Instantiate(
                    _levelBlocks[_levelContent[i*_gridSize.x + j]],
                    transform.position + Vector3.right * i + Vector3.forward * j,
                    transform.rotation,
                    transform
                );
            }
        }
    }
}
