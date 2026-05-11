using Unity.Cinemachine;
using UnityEngine;

public class FpsController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _sprintSpeed = 9;
    [SerializeField] private float _gravity = -15;
    [SerializeField] private float _jumpHeight = 1.2f;
    [SerializeField] private float _xSensitivity = 60;
    [SerializeField] private float _ySensitivity = 60;
    [SerializeField] CinemachineCamera _camera;
    public Vector3 Velocity { get => _velocity; private set => _velocity = value; }
    
    private CharacterController _controller;
    private InputManager _input;
    private Vector3 _velocity;
    private float _xRotation;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<InputManager>();
    }

    public void MoveAndFall(Vector2 input)
    {
        float speed = _input.IsSprinting ? _sprintSpeed : _moveSpeed;

        if (_controller.isGrounded && _velocity.y < 0)
            _velocity.y = -2;

        _controller.Move(transform.TransformDirection(new Vector3(input.x,0,input.y) * speed * Time.deltaTime));
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

    }


    public void Look(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        _xRotation -= (mouseY * Time.deltaTime) * _ySensitivity;
        _xRotation = Mathf.Clamp(_xRotation, -80, 80);

        _camera.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        Vector3 newRotation = Vector3.up * (mouseX * Time.deltaTime) * _xSensitivity;
        transform.Rotate(newRotation);
    }
    
}
