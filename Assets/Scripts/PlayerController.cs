using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Componentes
    private CharacterController _controller;
    //Inputs 
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _moveInput;
    //Variables
    [SerializeField] private float _jumpHeight = 2;
    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _smoothTime = 0.2f;
    private float _turnSmoothVelocity;
    //Gravedad
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 _playerGravity;
    //GroundSensor
    [SerializeField] private Transform _sensor;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _sensorRadius;





    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
    }

    void Start()
    {

    }


    void Update()
    {
        MovimientoCutre();

        if (_jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        Gravity();
    }

    void MovimientoCutre()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(_moveInput.x, 0, _moveInput.y);

        if (direction != Vector3.zero)
        {
            //Rad2Deg --> transforma angulos radianes a normales de toda la vida de 360
            //El Atan2 te devuelve el angulo  en el que estas pulsando, si pulsas arriba pues 0, si pulsas abajo 180, si pulsas derecha 90 y asi.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            //Hace que la transicion cuando giras sea mas smooth
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);
            //El direction.normalized hace que el vector tenga de limite 1, para que no se mueva mas rapido en digonal que en linea recta.
            _controller.Move(direction.normalized * _movementSpeed * Time.deltaTime);
        }

    }

    void Jump()
    {
        _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        _controller.Move(_playerGravity * Time.deltaTime);
    }

    void Gravity()
    {
        if (!IsGrounded())
        {
            //Añade la fuerza de gravedad a la y del personaje
            _playerGravity.y += _gravity * Time.deltaTime;
        }
        else if (IsGrounded() && _playerGravity.y < _gravity)
        {
            _playerGravity.y = _gravity;
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
