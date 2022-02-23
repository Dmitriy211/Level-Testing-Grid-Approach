using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1;
    [SerializeField] private float _jumpForce = 2;
    [SerializeField] private float _gravity = 10;
    [SerializeField] private LayerMask _groundMask;

    private Vector3 _moveVector;
    private CharacterController _characterController;
    private UnityEngine.InputSystem.PlayerInput _playerInput;
    private float _verticalVelocity;
    private bool _grounded;
    private bool _reset;
    private Vector2 _direction;
    private bool _lockControls;

    private void Awake()
    {
        _playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
        _characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        _playerInput.actions["Move"].performed += Move;
        _playerInput.actions["Move"].canceled += Move;
        _playerInput.actions["Jump"].performed += Jump;
    }

    private void Update()
    {
        CheckGround();
        Fall();
        if (_moveVector.sqrMagnitude > 0)
            _characterController.Move(
                transform.TransformVector(_moveVector) * _moveSpeed *
                Time.deltaTime);
    }

    public void Move(Vector2 moveVector)
    {
        _moveVector.z = moveVector.y;
        _moveVector.x = moveVector.x;
    }
    
    public void Move(InputAction.CallbackContext inputContext)
    {
        Move(inputContext.ReadValue<Vector2>());
    }
    
    public void Jump()
    {
        if (_grounded)
        {
            _verticalVelocity = _jumpForce;
        }
    }

    public void Jump(InputAction.CallbackContext inputContext)
    {
        Jump();
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
}
