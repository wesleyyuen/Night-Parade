using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WakizashiFSM : WeaponFSM
{
    private WakizashiIdleState _idleState;
    private WakizashiAttackState _attackState;
    private WakizashiParryState _parryState;
    private WakizashiBlockState _blockState;
    private WakizashiThrownState _thrownState;
    private WakizashiReturnState _returnState;
    private WakizashiLodgedState _lodgedState;
    private WakizashiFallState _fallState;
    [HideInInspector] public bool isThrownRight;

    protected override void Awake()
    {
        _idleState = new WakizashiIdleState(this);
        _attackState = new WakizashiAttackState(this);
        _parryState = new WakizashiParryState(this);
        _blockState = new WakizashiBlockState(this);
        _thrownState = new WakizashiThrownState(this);
        _returnState = new WakizashiReturnState(this);
        _lodgedState = new WakizashiLodgedState(this);
        _fallState = new WakizashiFallState(this);

        weaponData = new WakizashiData();

        isThrownRight = true;

        base.Awake();

        states.Add(StateType.IdleState, _idleState);
        states.Add(StateType.AttackState, _attackState);
        states.Add(StateType.ParryState, _parryState);
        states.Add(StateType.BlockState, _blockState);
        states.Add(StateType.ThrownState, _thrownState);
        states.Add(StateType.ReturnState, _returnState);
        states.Add(StateType.LodgedState, _lodgedState);
        states.Add(StateType.FallState, _fallState);
    }
    
    // TODO: shouldn't bind inputs like this, should be states' responsibility
    private void OnEnable()
    {
        _idleState.BindInput();
        _attackState.BindInput();
        _blockState.BindInput();
        _thrownState.BindInput();
        _lodgedState.BindInput();
        _fallState.BindInput();
    }

    private void OnDisable()
    {
        _idleState.UnbindInput();
        _attackState.UnbindInput();
        _blockState.UnbindInput();
        _thrownState.UnbindInput();
        _lodgedState.UnbindInput();
        _fallState.UnbindInput();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsCurrentState(StateType.ThrownState)) _thrownState.OnTriggerEnter2D(collider);
        else if (IsCurrentState(StateType.LodgedState)) _lodgedState.OnTriggerEnter2D(collider);
        else if (IsCurrentState(StateType.FallState)) _fallState.OnTriggerEnter2D(collider);
    }

    protected override void Start()
    {
        SetState(_idleState);
    }
}
