using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private float _jumpForce = 2;
    [SerializeField] private float _jumpTime = 1;
    [SerializeField] private float _attackDashForce = 1;
    [SerializeField] private float _attackTime = 1;
    [SerializeField] private GameObject _attackProjectile;
    [SerializeField] private float _gravity = 10;
    [SerializeField] private LayerMask _groundMask;

    private Vector3 _moveVector;
    private CharacterController _characterController;
    private UnityEngine.InputSystem.PlayerInput _playerInput;
    private float _verticalVelocity;
    private bool _grounded;
    private bool _lockControls; 
    private Vector3 _lookVector;

    private void Awake()
    {
        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _playerInput.actions["Move"].performed += Move;
        _playerInput.actions["Move"].canceled += Move;
        _playerInput.actions["Jump"].started += Jump;
        _playerInput.actions["Attack"].started += Attack;
    }

    private void OnDisable()
    {
        _playerInput.actions["Move"].performed -= Move;
        _playerInput.actions["Move"].canceled -= Move;
        _playerInput.actions["Jump"].started -= Jump;
        _playerInput.actions["Attack"].started -= Attack;
    }

    private void Update()
    {
        CheckGround();
        Fall();
        if (_moveVector.sqrMagnitude > 0 && !_lockControls)
        {
            _characterController.Move(_moveVector * _moveSpeed * Time.deltaTime);
            _lookVector = _moveVector.normalized;
            transform.forward = _lookVector;
        }
    }

    public void Move(Vector2 moveVector)
    {
        moveVector = moveVector.normalized;
        _moveVector.z = moveVector.y;
        _moveVector.x = moveVector.x;
    }
    
    public void Move(InputAction.CallbackContext inputContext)
    {
        Move(inputContext.ReadValue<Vector2>());
    }
    
    public void Jump()
    {
        if (_grounded && !_lockControls)
        {
            StartCoroutine(JumpRoutine());
        }
    }

    public void Jump(InputAction.CallbackContext inputContext)
    {
        Jump();
    }

    private IEnumerator JumpRoutine()
    {
        _lockControls = true;
        float initialSpeed = _jumpForce;
        float currentSpeed = initialSpeed;
        _verticalVelocity = _jumpForce / 2;
        float timeSpent = 0;
        
        while (timeSpent < _jumpTime)
        {
            _characterController.Move(_lookVector * currentSpeed * Time.deltaTime);
            currentSpeed = Mathf.Lerp(initialSpeed, 0, timeSpent / _jumpTime);
            timeSpent += Time.deltaTime;
            yield return null;
        }
        _lockControls = false;
    }
    
    public void Attack()
    {
        if (_grounded && !_lockControls)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    public void Attack(InputAction.CallbackContext inputContext)
    {
        Attack();
    }

    private IEnumerator AttackRoutine()
    {
        _lockControls = true;
        float initialSpeed = _attackDashForce;
        float currentSpeed = initialSpeed;
        float timeSpent = 0;
        
        Instantiate(_attackProjectile, transform.position, transform.rotation, transform);
        
        while (timeSpent < _attackTime)
        {
            _characterController.Move(_lookVector * currentSpeed * Time.deltaTime);
            currentSpeed = Mathf.Lerp(initialSpeed, 0, timeSpent / _attackTime);
            timeSpent += Time.deltaTime;
            yield return null;
        }
        _lockControls = false;
    }

    private void Fall()
    {
        if (!_grounded) _verticalVelocity -= _gravity * Time.deltaTime;
        else if (_verticalVelocity < 0) _verticalVelocity = 0;

        _characterController.Move(Vector3.up * _verticalVelocity * Time.deltaTime);
    }

    private void CheckGround()
    {
        _grounded = Physics.SphereCast(
            transform.position + Vector3.up * _characterController.radius * transform.lossyScale.x,
            _characterController.radius * transform.lossyScale.x,
            Vector3.down,
            out RaycastHit hit,
            0.001f + _characterController.skinWidth,
            _groundMask,
            QueryTriggerInteraction.Ignore
        );
    }

    public void Reset()
    {
        transform.localPosition = Vector3.zero;
    }
}
