using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngineInternal;
using Random = UnityEngine.Random;

public class SolverAgent : Agent
{
    [SerializeField] private LevelBuilder _levelBuilder;
    
    private PlayerController _playerController;
    private UnityEngine.InputSystem.PlayerInput _playerInput;
    private List<int> _visitedPlatforms = new List<int>();
    private BehaviorParameters _behaviorParameters;

    public override void Initialize()
    {
        base.Initialize();
        
        _playerController = GetComponent<PlayerController>();
        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _behaviorParameters = GetComponent<BehaviorParameters>();
        
        if (TryGetComponent(out BehaviorParameters behaviorParameters))
            _playerInput.enabled = behaviorParameters.BehaviorType switch
            {
                BehaviorType.HeuristicOnly => true,
                BehaviorType.InferenceOnly => false,
                BehaviorType.Default => false,
                _ => _playerInput.enabled
            };
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        _playerController.Move(Vector2.up * actionBuffers.DiscreteActions[0] + Vector2.right * actionBuffers.DiscreteActions[1]);
        
        if (actionBuffers.DiscreteActions[2] == 1)
            _playerController.Jump();
        
        if (actionBuffers.DiscreteActions[3] == 1)
            _playerController.Attack();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_levelBuilder.LevelContent.Select(block => (float)block).ToList());
        sensor.AddObservation(GetCurrentPosition());
    }

    private Vector2 GetCurrentPosition()
    {
        var localPosition = transform.localPosition;
        return Vector2.up * (int)localPosition.z + Vector2.right * (int)localPosition.x;
    }

    private void GiveReward()
    {
        
    }
}
