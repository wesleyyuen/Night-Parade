using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiIdleState : WeaponState
{
    public override void EnterState(WeaponFSM fsm)
    {
    }

    public override void Update(WeaponFSM fsm)
    {
        if (Input.GetButtonDown ("Attack") && fsm.canAttack && fsm.attackCooldownTimer <= 0) {
            fsm.SetState(fsm.states[WeaponFSM.StateType.AttackState]);
        } else if (Input.GetButtonDown ("Block") && fsm.canBlock && fsm.blockCooldownTimer <= 0 && fsm.abilityController.currStamina > 0) {
            fsm.SetState(fsm.states[WeaponFSM.StateType.ParryState]);
        }
    }

    public override void FixedUpdate(WeaponFSM fsm)
    {
    }

    public override void ExitState(WeaponFSM fsm)
    {
    }
}
