using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BowStateType : WeaponStateType
{
  public static readonly BowStateType Draw = new BowStateType("Draw");
//   public static readonly BowStateType Shoot = new BowStateType("Shoot");
  public static readonly BowStateType Nock = new BowStateType("Nock");

  private BowStateType(string value) : base(value)
  {
  }
}

public sealed class BowFSM : WeaponFSM
{
    private BowIdleState _idleState;
    private BowAttackState _attackState;
    private BowParryState _parryState;
    private BowBlockState _blockState;
    private BowDrawState _drawState;
    private BowNockState _nockState;
    private BowShootState _shootState;
    public GameObject arrowPrefab;
    [HideInInspector] public bool isShotRight;

    protected override void Awake()
    {
        _idleState = new BowIdleState(this);
        _attackState = new BowAttackState(this);
        _parryState = new BowParryState(this);
        _blockState = new BowBlockState(this);
        _drawState = new BowDrawState(this);
        _nockState = new BowNockState(this);
        // _shootState = new BowShootState(this);

        weaponData = new BowData();

        isShotRight = true;

        base.Awake();

        InputActions.Player.Shoot.Enable();

        states.Add(BowStateType.Idle, _idleState);
        states.Add(BowStateType.Attack, _attackState);
        states.Add(BowStateType.Parry, _parryState);
        states.Add(BowStateType.Block, _blockState);
        states.Add(BowStateType.Draw, _drawState);
        states.Add(BowStateType.Nock, _nockState);
        // states.Add(BowStateType.Shoot, _shootState);

        SetState(_idleState);
    }
    
    // TODO: shouldn't bind inputs like this, should be states' responsibility
    private void OnEnable()
    {
        _idleState.BindInput();
        _attackState.BindInput();
        _blockState.BindInput();
        _drawState.BindInput();
    }

    private void OnDisable()
    {
        _idleState.UnbindInput();
        _attackState.UnbindInput();
        _blockState.UnbindInput();
        _drawState.UnbindInput();
    }

    // Called from Animation frames
    protected override void Attack(int isListeningForNextAttack)
    {
        if (_currentState == states[WeaponStateType.Attack]) {
            if (_currentState is BowAttackState state) {
                state.Attack(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from Animation frames
    protected override void Upthrust(int isListeningForNextAttack)
    {
        if (_currentState == states[WeaponStateType.Attack]) {
            if (_currentState is BowAttackState state) {
                state.Upthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from Animation frames
    protected override void Downthrust(int isListeningForNextAttack)
    {
        if (_currentState == states[WeaponStateType.Attack]) {
            if (_currentState is BowAttackState state) {
                state.Downthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from animation frame
    protected override void EndAttack()
    {
        if (_currentState == states[WeaponStateType.Attack]) {
            if (_currentState is BowAttackState state) {
                state.EndAttack();
            }
        }
    }
}
