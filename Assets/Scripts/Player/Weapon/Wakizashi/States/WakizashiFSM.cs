using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiFSM : WeaponFSM
{
    public readonly WakizashiIdleState idleState = new WakizashiIdleState();
    public readonly WakizashiAttackState attackState = new WakizashiAttackState();
    public readonly WakizashiParryState parryState = new WakizashiParryState();
    public readonly WakizashiBlockState blockState = new WakizashiBlockState();

    protected override void Awake()
    {
        weaponData = new WakizashiData();
        base.Awake();

        states.Add(StateType.IdleState, idleState);
        states.Add(StateType.AttackState, attackState);
        states.Add(StateType.ParryState, parryState);
        states.Add(StateType.BlockState, blockState);

        foreach(var e in states)
        {
            if (e.Value != null)
                e.Value.Awake(this);
        }
    }

    protected override void Start()
    {
        SetState(idleState);
    }
}
