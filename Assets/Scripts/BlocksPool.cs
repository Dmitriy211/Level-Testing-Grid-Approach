using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BlocksPool : MonoBehaviour
{
    [FormerlySerializedAs("_levelBlocks")] [SerializeField] private LevelBlock[] _allBlocks;
    [SerializeField] private BlockType _type;
    [SerializeField] private List<LevelBlock> _blocks;
    [SerializeField] private Vector2Int _gridSize;

    public BlockType Type => _type;

    private void Awake()
    {
        InstantiateBlocks();
    }

    public LevelBlock GetBlock(int position) => _blocks[position];

    private void InstantiateBlocks()
    {
        for (int i = 0; i < _gridSize.x * _gridSize.y; i++)
        {
            Vector2Int coords = GridTools.IntToCoord(i, _gridSize);
        
            LevelBlock block = Instantiate(
                _allBlocks.FirstOrDefault(b => b.BlockType == _type),
                transform.position + Vector3.right * coords.x + Vector3.forward * coords.y,
                transform.rotation,
                transform
            );
            block.gameObject.SetActive(false);
            _blocks.Add(block);
        }
    }
}
