using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private BlocksPool[] _pools;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private int[] _levelContent;

    public BlocksPool[] Pools => _pools;
    public LevelBlock[] LevelObjects => _levelObjects;
    public int[] LevelContent => _levelContent;
    public Vector2Int GridSize => _gridSize;

    private LevelBlock[] _levelObjects;

    private void OnValidate()
    {
        if (_levelContent.Length > _gridSize.x * _gridSize.y)
        {
            int[] tmp = _levelContent;
            _levelContent = new int[_gridSize.x * _gridSize.y];

            for (int i = 0; i < _levelContent.Length; i++)
                _levelContent[i] = tmp[i];
        }
    }

    private void Awake()
    {
        _levelObjects = new LevelBlock[_levelContent.Length];
    }

    private void Start()
    {
        BuildLevel();
    }

    public void BuildLevel(bool clean = false)
    {
        for (int i = 0; i < _levelContent.Length; i++)
        {
            BuildObject(i, _levelContent[i], clean);
        }
    }

    public void BuildObject(int position, int blockType, bool clean = false)
    {
        if (!clean && _levelObjects[position] && (int)_levelObjects[position].BlockType == blockType)
            return;
        
        if (_levelObjects[position])
        {
            _levelObjects[position].gameObject.SetActive(false);
        }

        Vector2Int coords = GridTools.IntToCoord(position, _gridSize);

        _levelObjects[position] = _pools.FirstOrDefault(p => (int) p.Type == blockType).GetBlock(position);
        _levelObjects[position].gameObject.SetActive(true);
    }

    public void BuildObject(int position)
    {
        BuildObject(position, _levelContent[position]);
    }
}