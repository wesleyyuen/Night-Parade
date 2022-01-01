using UnityEngine;
using UnityEngine.InputSystem;

// Include both idle + moving states
public sealed class BowIdleState : IWeaponState, IBindInput
{
    enum TargetState
    {
        None,
        Attack,
        Parry,
        Draw
    }
    TargetState _targetState = TargetState.None;
    BowFSM _fsm;
    Rigidbody2D _rb;
    PlayerAbilityController _abilityController;

    public BowIdleState(BowFSM fsm)
    {
        _fsm = fsm;

        _abilityController = fsm.GetComponentInParent<PlayerAbilityController>();
        _rb = fsm.GetComponent<Rigidbody2D>();
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Attack.started += OnStartAttack;
        _fsm.InputActions.Player.Block.started += OnStartBlock;
        _fsm.InputActions.Player.Shoot.started += OnStartDraw;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Attack.started -= OnStartAttack;
        _fsm.InputActions.Player.Block.started -= OnStartBlock;
        _fsm.InputActions.Player.Shoot.started -= OnStartDraw;
    }

    public void EnterState()
    {
    }

    private void OnStartAttack(InputAction.CallbackContext context)
    {
        if (_fsm.IsCurrentState(BowStateType.Idle) && _fsm.canAttack && _fsm.attackCooldownTimer <= 0)
            _targetState = TargetState.Attack;
    }

    private void OnStartBlock(InputAction.CallbackContext context)
    {
        if (_fsm.IsCurrentState(BowStateType.Idle) && _fsm.canBlock && _fsm.blockCooldownTimer <= 0 && _abilityController.currStamina > 0)
            _targetState = TargetState.Parry;
    }

    private void OnStartDraw(InputAction.CallbackContext context)
    {
        // Allow pre-inputs in other states
        _targetState = TargetState.Draw;
    }

    public void Update()
    {
        switch (_targetState)
        {
            case TargetState.Attack:
                _fsm.SetState(_fsm.states[BowStateType.Attack]);
                return;

            case TargetState.Parry:
                _fsm.SetState(_fsm.states[BowStateType.Parry]);
                return;

            case TargetState.Draw:
                _fsm.SetState(_fsm.states[BowStateType.Draw]);
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
        // Reset here to avoid clearing pre-inputs upon EnterState()
        _targetState = TargetState.None;
    }
}
