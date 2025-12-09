using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Movement : MonoBehaviour
{
    //Componentes
    private CharacterController _controller;
    [SerializeField] private CapsuleCollider _collider;
    private Animator _animator;
    //Inputs 
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lookAction;
    private InputAction _AttackAction;
    private InputAction _dashAction;
    [SerializeField] private Vector2 _lookInput;
    private InputAction _grabAction;
    private Vector2 _moveInput;
    //Variables
    [SerializeField] private float _jumpHeight = 2;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _smoothTime = 0.1f;
    private float _turnSmoothVelocity;
    [SerializeField] private float _pushForce = 10;
    [SerializeField] private float _throwForce = 20;
    //Camera
    private Transform _mainCamera;
    [SerializeField] float _speedChangeRate = 10;
    float _speed;
    float _animationSpeed;
    float sprintSpeed = 10;
    bool isSprinting = false;
    float targetAngle;

    //Cambio de personajes
    [SerializeField] private GameObject _Chico;
    [SerializeField] private GameObject _Chica;
    private InputAction _changeAction;
    [SerializeField] private bool _ChicoActive = false;
    [SerializeField] private bool _ChicaActive = true;
    private bool _isChanging = false;

    //DASH
    [SerializeField] private float _dashSpeed = 20;
    private bool _isDashing = false;
    float _dashTimer;
    [SerializeField] float _dashDuration = 0.5f;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _moveAction = InputSystem.actions["Move"];
        _lookAction = InputSystem.actions["Look"];
        _changeAction = InputSystem.actions["Change"];
        _AttackAction = InputSystem.actions["Attack"];
        _dashAction = InputSystem.actions["Jump"];
        _mainCamera = Camera.main.transform;
        _animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        _lookInput = _lookAction.ReadValue<Vector2>();
        MovimientoHades();
        
        if(_changeAction.WasPressedThisFrame() && _isChanging == false)
        {
            _isChanging = true;
            StartCoroutine(Change());
        }
        if(_AttackAction.WasPressedThisFrame())
        {
            Attack();
        }
        if(_dashAction.WasPressedThisFrame() && _isDashing == false)
        {
            _isDashing = true;
            StartCoroutine(Dash());
        }
    }

    void MovimientoHades()
    {
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        /*Ray ray = Camera.main.ScreenPointToRay(_lookInput);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 playerForward = hit.point - transform.position;
            playerForward.y = 0;
            transform.forward = playerForward;
        }*/

        if (direction != Vector3.zero)
        {
            _controller.Move(direction.normalized * _movementSpeed * Time.deltaTime);
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
        }
    }

    IEnumerator Dash()
    {
        Vector3 dashDirection = new Vector3(_moveInput.x, 0, _moveInput.y);

        while(_dashTimer < _dashDuration && _isDashing)
        {
            _dashTimer += Time.deltaTime;

            if (dashDirection == Vector3.zero)
            {
                _controller.Move(transform.forward * _dashSpeed * Time.deltaTime);
                //Physics.IgnoreLayerCollision(layermask1, layermask2, true/false);
            }
            else
            {
                _controller.Move(dashDirection.normalized * _dashSpeed * Time.deltaTime);
            }

            yield return null;
        }

        
        _dashTimer = 0;
        _isDashing = false;

        
    }

    void Attack()
    {
        if(_ChicaActive)
        {
            Debug.Log("Ataque Chica");
        }
        if(_ChicoActive)
        {
            Debug.Log("Ataque Chico");
        }
    }

    IEnumerator Change()
    {
        if(_ChicaActive)
        {
            _ChicaActive = false;
            _ChicoActive = true;
            _Chica.SetActive(false);
            _Chico.SetActive(true);
            yield return new WaitForSeconds(1);
            _isChanging = false;
        }
        else if(_ChicoActive)
        {
            _ChicoActive = false;
            _ChicaActive = true;
            _Chico.SetActive(false);
            _Chica.SetActive(true);
            yield return new WaitForSeconds(1);
            _isChanging = false;
        }
    }
}