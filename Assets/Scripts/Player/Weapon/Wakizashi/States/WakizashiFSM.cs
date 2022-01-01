using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WakizashiStateType : WeaponStateType
{
  public static readonly WakizashiStateType Thrown = new WakizashiStateType("Thrown");
  public static readonly WakizashiStateType Return = new WakizashiStateType("Return");
  public static readonly WakizashiStateType Lodged = new WakizashiStateType("Lodged");
  public static readonly WakizashiStateType Fall = new WakizashiStateType("Fall");

  private WakizashiStateType(string value) : base(value)
  {
  }
}

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

        InputActions.Player.Throw.Enable();

        states.Add(WakizashiStateType.Idle, _idleState);
        states.Add(WakizashiStateType.Attack, _attackState);
        states.Add(WakizashiStateType.Parry, _parryState);
        states.Add(WakizashiStateType.Block, _blockState);
        states.Add(WakizashiStateType.Thrown, _thrownState);
        states.Add(WakizashiStateType.Return, _returnState);
        states.Add(WakizashiStateType.Lodged, _lodgedState);
        states.Add(WakizashiStateType.Fall, _fallState);

        SetState(_idleState);
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
        if (IsCurrentState(WakizashiStateType.Thrown))
            _thrownState.OnTriggerEnter2D(collider);
        else if (IsCurrentState(WakizashiStateType.Lodged))
            _lodgedState.OnTriggerEnter2D(collider);
        else if (IsCurrentState(WakizashiStateType.Fall))
            _fallState.OnTriggerEnter2D(collider);
    }


    // Called from Animation frames
    protected override void Attack(int isListeningForNextAttack)
    {
        if (_currentState == states[WeaponStateType.Attack]) {
            if (_currentState is WakizashiAttackState state) {
                state.Attack(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from Animation frames
    protected override void Upthrust(int isListeningForNextAttack)
    {
        if (_currentState == states[WeaponStateType.Attack]) {
            if (_currentState is WakizashiAttackState state) {
                state.Upthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from Animation frames
    protected override void Downthrust(int isListeningForNextAttack)
    {
        if (_currentState == states[WeaponStateType.Attack]) {
            if (_currentState is WakizashiAttackState state) {
                state.Downthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from animation frame
    protected override void EndAttack()
    {
        if (_currentState == states[WeaponStateType.Attack]) {
            if (_currentState is WakizashiAttackState state) {
                state.EndAttack();
            }
        }
    }

    public bool IsOnPlayer()
    {
        return !(IsCurrentState(WakizashiStateType.Thrown) || IsCurrentState(WakizashiStateType.Lodged));
    }

    public void UnlodgedFromEnemy()
    {
        if (IsCurrentState(WakizashiStateType.Lodged)) {
            SetState(states[WakizashiStateType.Fall]);
        }
    }
}
