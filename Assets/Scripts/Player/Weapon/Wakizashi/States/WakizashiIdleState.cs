using UnityEngine.InputSystem;

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
        // fsm.inputActions.Player.Attack.Enable();
        // fsm.inputActions.Player.Block.Enable();
    }

    void OnStartAttack(InputAction.CallbackContext context)
    {
        if (_fsm.canAttack && _fsm.attackCooldownTimer <= 0)
            _fsm.SetState(_fsm.states[WeaponFSM.StateType.AttackState]);

    }

    void OnStartBlock(InputAction.CallbackContext context)
    {
        if (_fsm.canBlock && _fsm.blockCooldownTimer <= 0 && _fsm.abilityController.currStamina > 0)
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
        // fsm.inputActions.Player.Attack.Disable();
        // fsm.inputActions.Player.Block.Disable();
    }
}
