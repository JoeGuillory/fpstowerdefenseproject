using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public Vector2 Move;
    public Vector2 Look;
    public bool IsJumping;
    public bool IsSprinting;

    public bool analogMovement;

    private PlayerActionMap _playerActionMap;
    private void Awake()
    {
        _playerActionMap = new PlayerActionMap();
    }

    private void OnEnable()
    {
       _playerActionMap.Enable();

       _playerActionMap.FpsPlayer.Move.performed += OnMove;
       _playerActionMap.FpsPlayer.Move.canceled += OnMove;
       
       _playerActionMap.FpsPlayer.Look.performed += OnLook;
       _playerActionMap.FpsPlayer.Look.canceled += OnLook;
       
       _playerActionMap.FpsPlayer.Jump.performed += OnJump;
       
       _playerActionMap.FpsPlayer.Sprint.performed += OnSprint;
       _playerActionMap.FpsPlayer.Sprint.canceled += OnSprint;
    }

    private void OnDisable()
    {
        _playerActionMap.FpsPlayer.Move.performed -= OnMove;
        _playerActionMap.FpsPlayer.Move.canceled -= OnMove;
       
        _playerActionMap.FpsPlayer.Look.performed -= OnLook;
        _playerActionMap.FpsPlayer.Look.canceled -= OnLook;
       
        _playerActionMap.FpsPlayer.Jump.performed -= OnJump;
        _playerActionMap.FpsPlayer.Jump.canceled -= OnJump;
       
        _playerActionMap.FpsPlayer.Sprint.performed -= OnSprint;
        _playerActionMap.FpsPlayer.Sprint.canceled -= OnSprint;
        
        _playerActionMap.Disable();
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        MoveInput(value.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        LookInput(value.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        JumpInput(value.ReadValueAsButton());
    }

    public void OnSprint(InputAction.CallbackContext value)
    {
        SprintInput(value.ReadValueAsButton());
    }

    public void MoveInput(Vector2 newMoveInput)
    {
        Move = newMoveInput;
    }

    public void LookInput(Vector2 newLookInput)
    {
        Look = newLookInput;
    }

    public void JumpInput(bool newJumpInput)
    {
        IsJumping = newJumpInput;
    }

    public void SprintInput(bool newSprintInput)
    {
        IsSprinting = newSprintInput;
    }


    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(hasFocus);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
