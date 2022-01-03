using UnityEngine;
using UnityEngine.InputSystem;

// Include both idle + moving states
public sealed class WakizashiIdleState : IWeaponState, IBindInput
{
    private enum TargetState
    {
        None,
        Attack,
        Parry,
        Throw
    }
    private TargetState _targetState;
    private WakizashiFSM _fsm;
    private Rigidbody2D _rb;
    private PlayerAbilityController _abilityController;

    public WakizashiIdleState(WakizashiFSM fsm)
    {
        _fsm = fsm;

        _abilityController = fsm.GetComponentInParent<PlayerAbilityController>();
        _rb = fsm.GetComponent<Rigidbody2D>();
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Attack.started += OnStartAttack;
        _fsm.InputActions.Player.Block.started += OnStartBlock;
        _fsm.InputActions.Player.Throw.started += OnStartThrow;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Attack.started -= OnStartAttack;
        _fsm.InputActions.Player.Block.started -= OnStartBlock;
        _fsm.InputActions.Player.Throw.started -= OnStartThrow;
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

    private void OnStartBlock(InputAction.CallbackContext context)
    {
        if (_fsm.IsCurrentState(WakizashiStateType.Idle) && _fsm.canBlock && _fsm.blockCooldownTimer <= 0 && _abilityController.currStamina > 0)
            _targetState = TargetState.Parry;
    }

    private void OnStartThrow(InputAction.CallbackContext context)
    {
        if (!_fsm.IsCurrentState(WakizashiStateType.Throw) && _fsm.throwCooldownTimer <= 0)
            _targetState = TargetState.Throw;
    }

    public void Update()
    {
        switch (_targetState)
        {
            case TargetState.Attack:
                _fsm.SetState(_fsm.states[WakizashiStateType.Attack]);
                return;

            case TargetState.Parry:
                _fsm.SetState(_fsm.states[WakizashiStateType.Parry]);
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
