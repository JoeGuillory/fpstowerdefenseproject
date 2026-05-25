using Unity.Cinemachine;
using Unity.Multiplayer.Center.NetcodeForGameObjectsExample;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsControllerSync : NetworkBehaviour
{

    [SerializeField] CinemachineCamera _camera;
    private InputManager _input;
    private FpsController _controller;
    private PlayerInput _playerInput;
    private ClientNetworkTransform _clientNetTransform;

    private void Awake()
    {
        _input = GetComponent<InputManager>();
        _controller = GetComponent<FpsController>();
        _playerInput = GetComponent<PlayerInput>();
        _clientNetTransform = GetComponent<ClientNetworkTransform>();
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            _input.enabled = false;
            _playerInput.enabled = false;
            _camera.enabled = false;
        }
        else
        {
            if (_clientNetTransform != null)
            {
                _clientNetTransform.Interpolate = false;
            }
        }
        base.OnNetworkSpawn();
    }
    
    private void Update()
    {
        if (IsOwner)
        {
            _controller.JumpAndFall();
            _controller.CheckGrounded();
            _controller.Move();
        }
    }

    private void LateUpdate()
    {
        if (IsOwner)
        {
            _controller.Look(_input.Look);
        }
    }
}
