using UnityEngine;
using UnityEngine.InputSystem;

// Include both idle + moving states
public sealed class WakizashiIdleState : IWeaponState, IBindInput
{
    private enum TargetState
    {
        None,
        Attack,
        Aim,
        Throw
    }
    private TargetState _targetState;
    private WakizashiFSM _fsm;
    private Rigidbody2D _rb;
    private PlayerAnimations _playerAnimation;
    private PlayerAbilityController _abilityController;

    public WakizashiIdleState(WakizashiFSM fsm)
    {
        _fsm = fsm;

        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
        _abilityController = fsm.GetComponentInParent<PlayerAbilityController>();
        _rb = fsm.GetComponent<Rigidbody2D>();
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Attack.started += OnStartAttack;
        _fsm.InputActions.Player.Throw_Tap.performed += OnStartThrow;
        _fsm.InputActions.Player.Throw_Hold.performed += OnStartAim;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Attack.started -= OnStartAttack;
        _fsm.InputActions.Player.Throw_Tap.performed += OnStartThrow;
        _fsm.InputActions.Player.Throw_Hold.performed -= OnStartAim;
    }

    public void EnterState()
    {
        _targetState = TargetState.None;

        // Reset forces and transform
        _rb.velocity = Vector2.zero;
        _fsm.transform.localPosition = Vector3.zero;
        _fsm.transform.localEulerAngles = Vector3.zero;
    }

    private void OnStartAttack(InputAction.CallbackContext context)
    {
        if (_fsm.IsCurrentState(WakizashiStateType.Idle) && _fsm.canAttack && _fsm.attackCooldownTimer <= 0)
            _targetState = TargetState.Attack;
    }

    private void OnStartThrow(InputAction.CallbackContext context)
    {
        if (_fsm.IsCurrentState(WakizashiStateType.Idle) && _fsm.throwCooldownTimer <= 0)
            _targetState = TargetState.Throw;
    }

    private void OnStartAim(InputAction.CallbackContext context)
    {
        if (_fsm.IsCurrentState(WakizashiStateType.Idle) && _fsm.throwCooldownTimer <= 0)
            _targetState = TargetState.Aim;
    }

    public void Update()
    {
        switch (_targetState)
        {
            case TargetState.Attack:
                _fsm.SetState(_fsm.states[WakizashiStateType.Attack]);
                return;

            case TargetState.Aim:
                _fsm.SetState(_fsm.states[WakizashiStateType.Aim]);
                return;

            case TargetState.Throw:
                _fsm.SetState(_fsm.states[WakizashiStateType.Throw]);
                return;
            
            case TargetState.None:
            default:
                return;
        }
    }

    public void FixedUpdate()
    {
    }

    public void ExitState()
    {
    }
}
