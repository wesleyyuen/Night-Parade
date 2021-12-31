using UnityEngine;
using UnityEngine.InputSystem;

// Include both idle + moving states
public sealed class WakizashiIdleState : IWeaponState, IBindInput
{
    enum TargetState
    {
        None,
        Attack,
        Parry,
        Throw
    }
    TargetState _targetState;
    WakizashiFSM _fsm;
    Rigidbody2D _rb;
    PlayerAbilityController _abilityController;

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
        _fsm.InputActions.Player.Throw.canceled += OnStartThrow;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Attack.started -= OnStartAttack;
        _fsm.InputActions.Player.Block.started -= OnStartBlock;
        _fsm.InputActions.Player.Throw.canceled -= OnStartThrow;
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
        if (_fsm != null && _fsm.canAttack && _fsm.attackCooldownTimer <= 0)
            _targetState = TargetState.Attack;
    }

    private void OnStartBlock(InputAction.CallbackContext context)
    {
        if (_fsm != null && _fsm.canBlock && _fsm.blockCooldownTimer <= 0 && _abilityController.currStamina > 0)
            _targetState = TargetState.Parry;
    }

    private void OnStartThrow(InputAction.CallbackContext context)
    {
        if (_fsm != null && !_fsm.IsCurrentState(WeaponFSM.StateType.ThrownState))
            _targetState = TargetState.Throw;
    }

    public void Update()
    {
        switch (_targetState)
        {
            case TargetState.Attack:
                _fsm.SetState(_fsm.states[WeaponFSM.StateType.AttackState]);
                return;

            case TargetState.Parry:
                _fsm.SetState(_fsm.states[WeaponFSM.StateType.ParryState]);
                return;

            case TargetState.Throw:
                _fsm.SetState(_fsm.states[WeaponFSM.StateType.ThrownState]);
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
