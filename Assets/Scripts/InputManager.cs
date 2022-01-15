using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    static InputManager instance;
    public static InputManager Instance {
        get  {return instance; }
    }

    public enum DirectionInput { None, Up, RightUp, Right, RightDown, Down, LeftDown, Left, LeftUp }
    InputMaster _input;
    
    // Events
    public static event System.Action Event_Input_Interact;
    public static event System.Action Event_Input_Jump;
    public static event System.Action Event_Input_JumpCanceled;
    public static event System.Action Event_Input_Dash;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad (gameObject); // Handled by Parent
        } else {
            Destroy (gameObject);
        }

        // Handle Input
        _input = new InputMaster();
    }

    private void OnEnable()
    {
        _input.Player.Enable();

        // Bind Callbacks
        BindInteractCallback();
        BindJumpCallback();
        BindDashCallback();
    }

    private void OnDisable()
    {
        _input.Player.Disable();

        // Unbind Callbacks
        UnbindInteractCallback();
        UnbindJumpCallback();
        UnbindDashCallback();
    }
    
#region Direction Input
    public int GetDirectionalInput()
    {
        DirectionInput dir = DirectionInput.Right;
        Vector2 input = GetDirectionalInputVector();
        if (Mathf.Approximately(input.x, 0f)) {
            if (Mathf.Approximately(input.y, 0f)) dir = DirectionInput.None;
            else if (input.y > 0f)                dir = DirectionInput.Up;
            else if (input.y < 0f)                dir = DirectionInput.Down;
        } else if (input.x > 0f) {
            if (Mathf.Approximately(input.y, 0f)) dir = DirectionInput.Right;
            else if (input.y > 0f)                dir = DirectionInput.RightUp;
            else if (input.y < 0f)                dir = DirectionInput.RightDown;
        } else if (input.x < 0f) {
            if (Mathf.Approximately(input.y, 0f)) dir = DirectionInput.Left;
            else if (input.y > 0f)                dir = DirectionInput.LeftUp;
            else if (input.y < 0f)                dir = DirectionInput.LeftDown;
        }

        return (int) dir;
    }

    public Vector2 GetDirectionalInputVector()
    {
        return _input.Player.Movement.ReadValue<Vector2>();
    }

    public bool HasDirectionalInput()
    {
        DirectionInput inputEnum = (DirectionInput) GetDirectionalInput();
        return inputEnum != DirectionInput.None;
    }

    public bool HasDirectionalInput(DirectionInput dir)
    {
        DirectionInput inputEnum = (DirectionInput) GetDirectionalInput();
        Vector2 inputVector = GetDirectionalInputVector();
        switch (dir)
        {
            case DirectionInput.Left:
                return inputVector.x < 0f;
            case DirectionInput.Right:
                return inputVector.x > 0f;
            default:
                return dir == inputEnum;
        }
    }
#endregion

#region Interact
    private void BindInteractCallback()
    {
        _input.Player.Interact.started += OnInteract;
    }

    private void UnbindInteractCallback()
    {
        _input.Player.Interact.started -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        Event_Input_Interact?.Invoke();
    }
#endregion

#region Jump
    private void BindJumpCallback()
    {
        _input.Player.Jump.started += OnJump;
        _input.Player.Jump.canceled += OnJumpCanceled;
    }

    private void UnbindJumpCallback()
    {
        _input.Player.Jump.started -= OnJump;
        _input.Player.Jump.canceled -= OnJumpCanceled;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Event_Input_Jump?.Invoke();
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        Event_Input_JumpCanceled?.Invoke();
    }

    public bool HasJumpInput()
    {
        return _input.Player.Jump.ReadValue<float>() > 0.5f;
    }
#endregion

#region Dash
    private void BindDashCallback()
    {
        _input.Player.Dash.started += OnDash;
    }

    private void UnbindDashCallback()
    {
        _input.Player.Dash.started -= OnDash;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        Event_Input_Dash?.Invoke();
    }
#endregion
}
