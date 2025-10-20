using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    private CharacterController _controller;
    private Animator _animator;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _moveInput;
    private InputAction _aimAction;
    [SerializeField] private float _jumpHeight = 2;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;
    [SerializeField] private Transform _sensor;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _sensorRadius;
    //Camara
    private Transform _mainCamera;
    private InputAction _lookAction;
    [SerializeField] private Vector2 _lookInput;
    [SerializeField] private float _cameraSens = 10;
    [SerializeField] private Transform _lookAtCamera;
    float xRotation;



    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _mainCamera = Camera.main.transform;

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _lookAction = InputSystem.actions["Look"];
        _aimAction = InputSystem.actions["Aim"];
        
    }
    void Jump()
    {
        _animator.SetBool("IsJumping", true);
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _lookInput = _lookAction.ReadValue<Vector2>();

        Movement();

        if (_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        Gravity();
    }

    void Movement()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);
        float mouseX = _lookInput.x * _cameraSens * Time.deltaTime;
        float mouseY = _lookInput.y * _cameraSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        _animator.SetFloat("Horizontal", _moveInput.x);
        _animator.SetFloat("Vertical", _moveInput.y);
        transform.Rotate(Vector3.up, mouseX);
        _lookAtCamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //_lookAtCamera.Rotate(Vector3.right, mouseY);

        if(direction != Vector3.zero)
        {
            float _targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0, _targetAngle, 0) * Vector3.forward;
            _controller.Move(moveDirection * _movementSpeed * Time.deltaTime);
        }
    }

    void Gravity()
    {
        if (!IsGrounded())
        {
            //Añade la fuerza de gravedad a la y del personaje
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if (IsGrounded() && _playerGravity.y < 0)
        {
            _playerGravity.y = _gravity;
            _animator.SetBool("IsJumping", false);
        }
        //Aplica la gravedad al personaje
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(_sensor.position, _sensorRadius, _groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_sensor.position, _sensorRadius);
    }
}
