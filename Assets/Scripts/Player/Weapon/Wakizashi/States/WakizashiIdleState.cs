using UnityEngine.InputSystem;

// Include both idle + moving states
public class WakizashiIdleState : IWeaponState
{
    WakizashiFSM _fsm;
    public void Awake(WeaponFSM fsm)
    {
        _fsm = (WakizashiFSM) fsm;
        fsm.inputActions.Player.Attack.started += OnStartAttack;
        fsm.inputActions.Player.Block.started += OnStartBlock;
    }

    public void EnterState(WeaponFSM fsm)
    {
    }

    void OnStartAttack(InputAction.CallbackContext context)
    {
        if (_fsm != null && _fsm.canAttack && _fsm.attackCooldownTimer <= 0)
            _fsm.SetState(_fsm.states[WeaponFSM.StateType.AttackState]);

    }

    void OnStartBlock(InputAction.CallbackContext context)
    {
        if (_fsm != null && _fsm.canBlock && _fsm.blockCooldownTimer <= 0 && _fsm.abilityController.currStamina > 0)
            _fsm.SetState(_fsm.states[WeaponFSM.StateType.ParryState]);
    }


    public void Update(WeaponFSM fsm)
    {
    }

    public void FixedUpdate(WeaponFSM fsm)
    {
    }

    public void ExitState(WeaponFSM fsm)
    {
    }
}
