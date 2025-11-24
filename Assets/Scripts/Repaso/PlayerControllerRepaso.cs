using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerRepaso : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _moveValue;
    [SerializeField] private float _playerVelocity = 5;

    //Salto
    [SerializeField] private float _jumpHeight = 2;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] Vector3 _playerGravity;

    //GroundSensor 
    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius;
    [SerializeField] private LayerMask _groundLayer;


    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];

    }

    
    void Update()
    {
        _moveValue = _moveAction.ReadValue<Vector2>();

        Movement();

        if(_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        Gravity();
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_moveValue.x, 0, _moveValue.y);

        _controller.Move(direction * _playerVelocity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);
    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * _gravity * -2);
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Gravity()
    {
       if(!IsGrounded())
        {
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if(IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravity;
        }
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_sensorPosition.position, _sensorRadius);
    }
}
