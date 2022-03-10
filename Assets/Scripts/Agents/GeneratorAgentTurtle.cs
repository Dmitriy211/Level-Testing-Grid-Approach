using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineInternal;
using Random = UnityEngine.Random;

public class GeneratorAgentTurtle : Agent
{
    [SerializeField] private bool _dynamicBuild;
    
    public UnityEvent EpisodeEnded;
    
    private LevelBuilder _levelBuilder;
    private int _currentPosition;

    private void Awake()
    {
        _levelBuilder = GetComponent<LevelBuilder>();
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        MoveTurtle(actionBuffers.DiscreteActions[0]);
        ChangeBlock(_currentPosition, actionBuffers.DiscreteActions[1]);
        
        if (StepCount >= MaxStep - 1)
        {
            GiveReward();
            if (!_dynamicBuild)
                _levelBuilder.BuildLevel();
            
            EpisodeEnded?.Invoke();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_levelBuilder.LevelContent.Select(block => (float)block).ToList());
    }
    
    public override void OnEpisodeBegin()
    {
        Reset();
    }

    private void MoveTurtle(Vector2Int moveVector)
    {
        Vector2Int gridSize = _levelBuilder.GridSize;
        Vector2Int newCoordinate = GridTools.IntToCoord(_currentPosition, gridSize) + moveVector;
        newCoordinate.x = Mathf.Clamp(newCoordinate.x, 0, gridSize.x - 1);
        newCoordinate.y = Mathf.Clamp(newCoordinate.y, 0, gridSize.y - 1);
        _currentPosition = GridTools.CoordToInt(newCoordinate, gridSize);
    }

    private void MoveTurtle(int direction)
    {
        switch (direction)
        {
            case 0:
                MoveTurtle(Vector2Int.up);
                break;
            case 1:
                MoveTurtle(Vector2Int.right);
                break;
            case 2:
                MoveTurtle(Vector2Int.down);
                break;
            case 3:
                MoveTurtle(Vector2Int.left);
                break;
        }
    }

    private void ChangeBlock(int position, int blockType)
    {
        _levelBuilder.LevelContent[position] = blockType;
        
        if (_dynamicBuild)
            _levelBuilder.BuildObject(position, blockType, false);
    }
    
    public void GenerateRandomLevel()
    {
        for (int i = 0; i < _levelBuilder.LevelContent.Length; i++)
        {
            _levelBuilder.LevelContent[i] = Random.Range(0, _levelBuilder.Pools.Length);
        }
    }

    private void GiveReward()
    {
        AddReward(-Mathf.Abs(_levelBuilder.LevelContent.Count(b => b == (int) BlockType.Player) - 1) * 0.1f + 1);
        AddReward(-Mathf.Abs(_levelBuilder.LevelContent.Count(b => b == (int) BlockType.Door) - 1) * 0.1f + 1);
    }

    private void Reset()
    {
        GenerateRandomLevel();
        
        if (_dynamicBuild)
            _levelBuilder.BuildLevel();
    }
}
