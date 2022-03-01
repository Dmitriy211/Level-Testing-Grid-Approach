using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class GeneratorAgentTurtle : Agent
{
    private LevelBuilder _levelBuilder;
    private int _currentPosition;

    private void Awake()
    {
        _levelBuilder = GetComponent<LevelBuilder>();
    }

    public override void OnActionReceived(float[] actionVector)
    {
        // 2 actions for move turtle
        // 1 action to change block
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        foreach (int blockType in _levelBuilder.LevelContent)
        {
            sensor.AddObservation(blockType);
        }
    }

    private void MoveTurtle(Vector2Int moveVector)
    {
        Vector2Int gridSize = _levelBuilder.GridSize;
        Vector2Int newCoordinate = GridTools.IntToCoord(_currentPosition, gridSize) + moveVector;
        _currentPosition = GridTools.CoordToInt(newCoordinate, gridSize);
    }

    private void ChangeBlock(int position, int blockType) =>
        _levelBuilder.LevelContent[position] = blockType;
    
    public void GenerateRandomLevel()
    {
        for (int i = 0; i < _levelBuilder.LevelContent.Length; i++)
        {
            _levelBuilder.LevelContent[i] = Random.Range(0, _levelBuilder.LevelBlocks.Length);
        }
    }

    private void Reset()
    {
        
    }
}
