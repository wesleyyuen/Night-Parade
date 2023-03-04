using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : InputMaster.IUIActions, InputMaster.IGameplayActions, InputMaster.ICheatsActions
{
    private InputMaster _input;
    public enum DirectionInput { None, Up, RightUp, Right, RightDown, Down, LeftDown, Left, LeftUp }

    public InputManager()
    {
        if (_input == null) {
            _input = new InputMaster();

            _input.UI.SetCallbacks(this);
            _input.Gameplay.SetCallbacks(this);
            _input.Cheats.SetCallbacks(this);
        }

        EnableGameplayInput();

#if UNITY_EDITOR
        _input.Cheats.Enable();
#endif
    }

    ~InputManager()
    {
        _input.UI.Disable();
    }

    public void EnableGameplayInput()
    {
        _input.UI.Enable();
        _input.Gameplay.Enable();
    }

    public void EnableMenuInput()
    {
        _input.UI.Enable();
        _input.Gameplay.Disable();
    }
    

    // UI Input
    //=========================================================================================
    public event Action Event_UIInput_Pause;
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed) {
            Event_UIInput_Pause?.Invoke();
        }   
    }

    // Gameplay Input
    //=========================================================================================
    public event Action<Vector2> Event_GameplayInput_Move;
    public void OnMovement(InputAction.CallbackContext context)
    {
        Event_GameplayInput_Move?.Invoke(context.ReadValue<Vector2>());
    }

    public event Action Event_GameplayInput_Interact;
        public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed) {
            Event_GameplayInput_Interact?.Invoke();
        }
    }

    //=========================================================================================

    public event Action Event_GameplayInput_Jump, Event_GameplayInput_JumpCanceled;
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) {
            Event_GameplayInput_Jump?.Invoke();
        } else if (context.canceled) {
            Event_GameplayInput_JumpCanceled?.Invoke();
        }
    }

    public bool HasJumpInput()
    {
        return _input.Gameplay.Jump.ReadValue<float>() > 0.5f;
    }

    //=========================================================================================

    public event Action Event_GameplayInput_Attack, Event_GameplayInput_ChargeAttack;
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) {
            Event_GameplayInput_Attack?.Invoke();
        } else if (context.performed) {
            Event_GameplayInput_ChargeAttack?.Invoke();
        }
    }

    //=========================================================================================

    public event Action Event_GameplayInput_Block, Event_GameplayInput_BlockCanceled;
    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.started) {
            Event_GameplayInput_Block?.Invoke();
        } else {
            Event_GameplayInput_BlockCanceled?.Invoke();
        }
    }

    public bool HasBlockInput()
    {
        return _input.Gameplay.Block.ReadValue<float>() > 0.5f;
    }

    //=========================================================================================

    public event Action Event_GameplayInput_ThrowTap, Event_GameplayInput_ThrowSlowTap, Event_GameplayInput_ThrowHold;
    public void OnThrowTap(InputAction.CallbackContext context)
    {
        if (context.started) {
            Event_GameplayInput_ThrowTap?.Invoke();
        }
        // if (context.started) Debug.Log("<color=green>ThrowTap - Started</color>");
        // else if (context.performed) Debug.Log("<color=green>ThrowTap - Performed</color>");
        // else if (context.canceled) Debug.Log("<color=green>ThrowTap - Canceled</color>");

    }

    public void OnThrowSlowTap(InputAction.CallbackContext context)
    {
        // if (context.performed) {
        //     Event_GameplayInput_ThrowSlowTap?.Invoke();
        // }
        // if (context.started) Debug.Log("<color=blue>ThrowSlowTap - Started</color>");
        // else if (context.performed) Debug.Log("<color=blue>ThrowSlowTap - Performed</color>");
        // else if (context.canceled) Debug.Log("<color=blue>ThrowSlowTap - Canceled</color>");
    }

    public void OnThrowHold(InputAction.CallbackContext context)
    {
        // if (context.performed) {
        //     Event_GameplayInput_ThrowHold?.Invoke();
        // }
        // if (context.started) Debug.Log("<color=purple>ThrowHold - Started</color>");
        // else if (context.performed) Debug.Log("<color=purple>ThrowHold - Performed</color>");
        // else if (context.canceled) Debug.Log("<color=purple>ThrowHold - Canceled</color>");
    }

    //=========================================================================================

    public event Action Event_GameplayInput_Dash;
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed) {
            Event_GameplayInput_Dash?.Invoke();
        }
    }

    //=========================================================================================

    public event Action Event_GameplayInput_Rewind;
    public void OnRewind(InputAction.CallbackContext context)
    {
        if (context.performed) {
            Event_GameplayInput_Rewind?.Invoke();
        }
    }

    //=========================================================================================

    public event Action Event_CheatInput_TestChamber;
    public void OnTestChamber(InputAction.CallbackContext context)
    {
#if UNITY_EDITOR
        if (context.performed) {
            Event_CheatInput_TestChamber?.Invoke();
        }
#endif
    }

    //=========================================================================================

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
        return _input.Gameplay.Movement.ReadValue<Vector2>();
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
}