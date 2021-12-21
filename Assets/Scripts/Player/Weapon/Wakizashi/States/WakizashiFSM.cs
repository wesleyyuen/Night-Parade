using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiFSM : WeaponFSM
{
    WakizashiIdleState _idleState;
    WakizashiAttackState attackState;
    WakizashiParryState parryState;
    WakizashiBlockState blockState;
    WakizashiThrownState thrownState;
    WakizashiLodgedState lodgedState;

    protected override void Awake()
    {
        _idleState = new WakizashiIdleState(this);
        attackState = new WakizashiAttackState(this);
        parryState = new WakizashiParryState(this);
        blockState = new WakizashiBlockState(this);
        thrownState = new WakizashiThrownState(this);
        lodgedState = new WakizashiLodgedState(this);

        weaponData = new WakizashiData();

        base.Awake();

        states.Add(StateType.IdleState, _idleState);
        states.Add(StateType.AttackState, attackState);
        states.Add(StateType.ParryState, parryState);
        states.Add(StateType.BlockState, blockState);
        states.Add(StateType.ThrownState, thrownState);
        states.Add(StateType.LodgedState, lodgedState);
    }
    
    void OnEnable()
    {
        _idleState.BindInput();
        attackState.BindInput();
        blockState.BindInput();
    }

    void OnDisable()
    {
        _idleState.UnbindInput();
        attackState.UnbindInput();
        blockState.UnbindInput();
    }

    protected override void Start()
    {
        SetState(_idleState);
    }
}
