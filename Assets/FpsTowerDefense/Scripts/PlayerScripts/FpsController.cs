using Unity.Cinemachine;
using UnityEngine;

public class FpsController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _sprintSpeed = 9;
    [SerializeField] private float _speedChangeRate = 5;
    [SerializeField] private float _gravity = -15;
    [SerializeField] private float _jumpHeight = 1.2f;
    [SerializeField] private float _jumpTimeout = .5f;
    [SerializeField] private float _fallTimeout = .15f;
    [SerializeField] private float _xSensitivity = 60;
    [SerializeField] private float _ySensitivity = 60;
    [SerializeField] CinemachineCamera _camera;
    
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    private bool _isGrounded;
    private float _speed;
    private float _terminalVelocity = 53.0f;
    
    private CharacterController _controller;
    private InputManager _input;
    private float _verticalVelocity;
    private float _xRotation;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<InputManager>();
        
        _jumpTimeoutDelta = _jumpTimeout;
        _fallTimeoutDelta = _fallTimeout;
    }
    
    public void JumpAndFall()
    {
        if (_isGrounded)
        {
            _fallTimeoutDelta = _fallTimeout;

            if (_verticalVelocity > 0)
            {
                _verticalVelocity = -2f;
            }

            if (_input.IsJumping && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = _jumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            
            _input.IsJumping = false;
        }
        
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
    }

    public void CheckGrounded()
    {
        _isGrounded = _controller.isGrounded;
    }
    public void Move()
    {
        float targetSpeed = _input.IsSprinting ? _sprintSpeed : _moveSpeed;

        if (_input.Move == Vector2.zero) targetSpeed = 0;
       
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        
        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.Move.magnitude : 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                _speedChangeRate * Time.deltaTime);

            _speed = Mathf.Round((_speed * 1000) / 1000);
        }
        else
        {
            _speed = targetSpeed;
        }
        Vector3 move = (transform.right * _input.Move.x + transform.forward * _input.Move.y).normalized;
        _controller.Move(_speed * Time.deltaTime * move + new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
    }


    public void Look(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        _xRotation -= (mouseY * Time.deltaTime) * _ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80, 80);

        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        Vector3 newRotation = (mouseX * Time.deltaTime) * _xSensitivity * Vector3.up;
        transform.Rotate(newRotation);
    }
    
}
