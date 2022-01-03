using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BowStateType : WeaponStateType
{
  public static readonly BowStateType Draw = new BowStateType("Draw");
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
    public float drawArrowCooldownTimer; 

    protected override void Awake()
    {
        weaponData = new BowData();

        _idleState = new BowIdleState(this);
        _attackState = new BowAttackState(this);
        _parryState = new BowParryState(this);
        _blockState = new BowBlockState(this);
        _drawState = new BowDrawState(this);
        _nockState = new BowNockState(this);

        isShotRight = true;
        BowData data = (BowData)weaponData;
        drawArrowCooldownTimer = data.drawArrowCooldown;

        base.Awake();

        InputActions.Player.Shoot_Hold.Enable();
        InputActions.Player.Shoot_SlowTap.Enable();

        states.Add(BowStateType.Idle, _idleState);
        states.Add(BowStateType.Attack, _attackState);
        states.Add(BowStateType.Parry, _parryState);
        states.Add(BowStateType.Block, _blockState);
        states.Add(BowStateType.Draw, _drawState);
        states.Add(BowStateType.Nock, _nockState);

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

    protected override void Update()
    {
        base.Update();

        // Handle Bow Draw Cooldown
        if (_currentState != states[BowStateType.Draw] && drawArrowCooldownTimer > 0f) {
            drawArrowCooldownTimer -= Time.deltaTime;
        }
    }

    // Called from Animation frames
    protected override void Attack(int isListeningForNextAttack)
    {
        if (_currentState == states[BowStateType.Attack]) {
            if (_currentState is BowAttackState state) {
                state.Attack(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from Animation frames
    protected override void Upthrust(int isListeningForNextAttack)
    {
        if (_currentState == states[BowStateType.Attack]) {
            if (_currentState is BowAttackState state) {
                state.Upthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from Animation frames
    protected override void Downthrust(int isListeningForNextAttack)
    {
        if (_currentState == states[BowStateType.Attack]) {
            if (_currentState is BowAttackState state) {
                state.Downthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from animation frame
    protected override void EndAttack()
    {
        if (_currentState == states[BowStateType.Attack]) {
            if (_currentState is BowAttackState state) {
                state.EndAttack();
            }
        }
    }
}
