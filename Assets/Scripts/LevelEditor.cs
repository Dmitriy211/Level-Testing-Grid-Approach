using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelBuilder))]
public class LevelEditor : MonoBehaviour
{
    [SerializeField] private LevelTile _tilePrefab;
    [SerializeField] private BlockType _currentBlockType;
    [SerializeField] private bool _mouseHeld;

    public bool MouseHeld
    {
        get => _mouseHeld;
        set => _mouseHeld = value;
    }

    private LevelBuilder _levelBuilder;

    private void Awake()
    {
        _levelBuilder = GetComponent<LevelBuilder>();
    }

    private void Start()
    {
        CreateTileGrid();
    }

    public void SetBlockType(int type)
    {
        _currentBlockType = (BlockType) type;
    }

    private void CreateTileGrid()
    {
        Vector2Int gridSize = _levelBuilder.GridSize;
        
        Transform tileParent = new GameObject().transform;
        tileParent.parent = transform;
        tileParent.name = "Tiles";

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                LevelTile tile = Instantiate(
                    _tilePrefab,
                    transform.position + Vector3.right * i + Vector3.forward * j + Vector3.up * 0.01f,
                    _tilePrefab.transform.rotation,
                    tileParent
                );
                tile.IsMain = (i + j) % 2 == 0;
                tile.Position = GridTools.CoordToInt(new Vector2Int(i, j), gridSize);
            }
        }
    }

    public void OnTileHover(int position)
    {
        if (_mouseHeld) EditTile(position);
    }
    
    public void OnTileClick(int position)
    {
        EditTile(position);
    }

    private void EditTile(int position)
    {
        _levelBuilder.LevelContent[position] = (int)_currentBlockType;
        _levelBuilder.BuildObject(position);
    }
}
