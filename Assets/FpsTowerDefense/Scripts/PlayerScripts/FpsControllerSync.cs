using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsControllerSync : NetworkBehaviour
{

    [SerializeField] CinemachineCamera _camera;
    private InputManager _input;
    private FpsController _controller;
    private PlayerInput _playerInput;

    private NetworkVariable<Vector3> _position = new NetworkVariable<Vector3>();
    private NetworkVariable<float> _rotation = new NetworkVariable<float>();
    private void Awake()
    {
        _input = GetComponent<InputManager>();
        _controller = GetComponent<FpsController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    public override void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            _input.enabled = false;
            _playerInput.enabled = false;
            _camera.enabled = false;
        }
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        if (!IsOwner)
        {
            transform.position = _position.Value;
            return;
        }

        _controller.MoveAndFall(_input.Move);

        MoveRpc(_input.Move);
    }

    private void LateUpdate()
    {
        if (!IsOwner)
        {
            transform.rotation = Quaternion.Euler(0, _rotation.Value, 0);
            return;
        }

        _controller.Look(_input.Look);

        LookRpc(_input.Look);
    }

    [Rpc(SendTo.Server)]
    void MoveRpc(Vector2 input)
    {
        _controller.MoveAndFall(input);
        _position.Value = transform.position;
    }

    [Rpc(SendTo.Server)]
    void LookRpc(Vector2 Input)
    {
        _controller.Look(Input);

        _rotation.Value = transform.rotation.eulerAngles.y;
    }
}
