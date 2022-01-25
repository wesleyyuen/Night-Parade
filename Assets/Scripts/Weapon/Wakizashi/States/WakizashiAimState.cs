using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class WakizashiAimState : IWeaponState, IBindInput
{
    private WakizashiFSM _fsm;
    private PlayerAnimations _playerAnimation;
    private bool _isThrowing, _stopUpdating;

    public WakizashiAimState(WakizashiFSM fsm)
    {
        _fsm = fsm;

        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Throw_SlowTap.performed += OnStartThrow;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Throw_SlowTap.performed -= OnStartThrow;
    }
 
    public void EnterState()
    {
        _isThrowing = false;
        _stopUpdating = false;

        bool hasInput = InputManager.Instance.HasDirectionalInput();
        _fsm.throwDirection = hasInput ? InputManager.Instance.GetDirectionalInputVector() : _playerAnimation.IsFacingRight() ? Vector2.right : Vector2.left;

        Utility.FreezePlayer(true);
        _playerAnimation.FreezePlayerAnimation(true);
    }

    private void OnStartThrow(InputAction.CallbackContext context)
    {
        if (_fsm.IsCurrentState(WakizashiStateType.Aim) && !_isThrowing)
            _isThrowing = true;
    }

    public void Update()
    {
        if (_stopUpdating) return;

        if (InputManager.Instance.HasDirectionalInput()) {
            _fsm.throwDirection = InputManager.Instance.GetDirectionalInputVector();
        }

        if (_isThrowing) {
            _stopUpdating = true;
            _fsm.SetState(_fsm.states[WakizashiStateType.Throw]);
        }
    }

    public void FixedUpdate()
    {
    }

    public void ExitState()
    {
        Utility.FreezePlayer(false);
        _playerAnimation.FreezePlayerAnimation(false);
    }
}
