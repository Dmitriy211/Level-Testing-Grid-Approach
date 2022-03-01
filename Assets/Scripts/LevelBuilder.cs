using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private LevelBlock[] _levelBlocks;
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private int[] _levelContent;

    public LevelBlock[] LevelBlocks => _levelBlocks;
    public int[] LevelContent => _levelContent;
    public Vector2Int GridSize => _gridSize;

    private LevelBlock[] _levelObjects;

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
        _levelObjects = new LevelBlock[_levelContent.Length];
    }

    public void BuildLevel(bool clean)
    {
        for (int i = 0; i < _levelContent.Length; i++)
        {
            BuildObject(i, _levelContent[i], clean);
        }
    }

    public void BuildObject(int position, int blockType, bool clean)
    {
        if (!clean && _levelObjects[position] && _levelObjects[position].BlockType ==
            _levelBlocks.FirstOrDefault(b => (int) b.BlockType == _levelContent[position]).BlockType)
            return;
        
        if (_levelObjects[position])
            Destroy(_levelObjects[position].gameObject);

        Vector2Int coords = GridTools.IntToCoord(position, _gridSize);
        
        _levelObjects[position] = Instantiate(
            _levelBlocks.FirstOrDefault(b => (int)b.BlockType == blockType),
            transform.position + Vector3.right * coords.x + Vector3.forward * coords.y,
            transform.rotation,
            transform
        );
    }
}