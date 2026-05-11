using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public Vector2 Move;
    public Vector2 Look;
    public bool IsJumping;
    public bool IsSprinting;

  
    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void MoveInput(Vector2 NewMoveInput)
    {
        Move = NewMoveInput;
    }

    public void LookInput(Vector2 NewLookInput)
    {
        Look = NewLookInput;
    }

    public void JumpInput(bool NewJumpInput)
    {
        IsJumping = NewJumpInput;
    }

    public void SprintInput(bool NewSprintInput)
    {
        IsSprinting = NewSprintInput;
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
