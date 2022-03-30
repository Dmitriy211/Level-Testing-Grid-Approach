using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SolverAgent : Agent
{
    [SerializeField] private PlayerCharacteristics _playerCharacteristics;
    
    public UnityEvent EpisodeEnded;
    
    private LevelBuilder _levelBuilder;
    private LevelManager _levelManager;
    private PlayerController _playerController;
    private PlayerStatistics _playerStatistics;
    private UnityEngine.InputSystem.PlayerInput _playerInput;

    private int[] _heuristicActions = {1, 1, 0, 0};

    public override void Initialize()
    {
        base.Initialize();
        
        _playerController = GetComponent<PlayerController>();
        _playerStatistics = GetComponent<PlayerStatistics>();
        _levelBuilder = GetComponentInParent<LevelBuilder>();
        
        _levelManager = GetComponentInParent<LevelManager>();

        SubscribeToReward();

        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        if (TryGetComponent(out BehaviorParameters behaviorParameters))
            _playerInput.enabled = behaviorParameters.BehaviorType switch
            {
                BehaviorType.HeuristicOnly => true,
                BehaviorType.InferenceOnly => false,
                BehaviorType.Default => false,
                _ => _playerInput.enabled
            };
        
        if (_playerInput.enabled) SubscribeToInput();
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // print($"{actionBuffers.DiscreteActions[0]} {actionBuffers.DiscreteActions[1]} {actionBuffers.DiscreteActions[2]} {actionBuffers.DiscreteActions[3]}");
        
        _playerController.Move(Vector2.up * (actionBuffers.DiscreteActions[0] - 1) + Vector2.right *
            (actionBuffers.DiscreteActions[1] - 1));
        
        // if (actionBuffers.DiscreteActions[2] == 1)
        //     _playerController.Jump();
        //
        // if (actionBuffers.DiscreteActions[3] == 1)
        //     _playerController.Attack();
        
        AddReward((-1f + -3f * _playerCharacteristics.Achiever) / MaxStep);

        if (StepCount >= MaxStep - 1)
        {
            EpisodeEnded?.Invoke();
            Debug.Log("Time is out");
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = _heuristicActions[0];
        discreteActionsOut[1] = _heuristicActions[1];
        // discreteActionsOut[2] = _heuristicActions[2];
        // discreteActionsOut[3] = _heuristicActions[3];
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_levelBuilder.CurrentLevelContent.Select(block => (float)block).ToList());
        sensor.AddObservation(GetCurrentPosition());
        
        // sensor.AddObservation(_playerCharacteristics.Killer);
        sensor.AddObservation(_playerCharacteristics.Achiever);
        // sensor.AddObservation(_playerCharacteristics.Socializer);
        sensor.AddObservation(_playerCharacteristics.Explorer);
    }

    public override void OnEpisodeBegin()
    {
        GeneratePlayerCharacteristics();
    }

    private void GeneratePlayerCharacteristics()
    {
        int type = Random.Range(0, 2);
        
        _playerCharacteristics = new PlayerCharacteristics(
            type == 0 ? 1 : 0,
            type == 1 ? 1 : 0,
            type == 2 ? 1 : 0,
            type == 3 ? 1 : 0
        );
    }

    private void SubscribeToReward()
    {
        _playerStatistics.CoinCollected.AddListener(() => AddReward(0.1f + 0.3f * _playerCharacteristics.Explorer));
        _levelManager.Won.AddListener(() => AddReward(1f + 3 * _playerCharacteristics.Achiever));
        
        _levelManager.Lost.AddListener(() => AddReward((MaxStep - StepCount) * (-1f + -3f * _playerCharacteristics.Achiever) / MaxStep));
    }

    private void SubscribeToInput()
    {
        _playerInput.actions["Move"].performed += SetAction;
        _playerInput.actions["Move"].canceled += SetAction;
        // _playerInput.actions["Jump"].started += SetAction;
        // _playerInput.actions["Attack"].started += SetAction;
    }

    private void SetAction(InputAction.CallbackContext inputContext)
    {
        switch (inputContext.action.name)
        {
            case "Move":
                _heuristicActions[0] = Mathf.RoundToInt(inputContext.ReadValue<Vector2>().y + 1);
                _heuristicActions[1] = Mathf.RoundToInt(inputContext.ReadValue<Vector2>().x + 1);
                break;
            // case "Jump":
            //     _heuristicActions[2] = (int)inputContext.ReadValue<float>();
            //     break;
            // case "Attack":
            //     _heuristicActions[3] = (int)inputContext.ReadValue<float>();
            //     break;
        }
    }

    private Vector2 GetCurrentPosition()
    {
        var localPosition = transform.localPosition;
        // return _levelBuilder.GridSize.x * (int)localPosition.z + (int)localPosition.x;
        return Vector2.up * localPosition.z + Vector2.right * localPosition.x;
    }

    public void RestartLevel()
    {
        _levelManager.ResetGame();
    }
}
