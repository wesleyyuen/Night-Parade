using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class WakizashiStateType : WeaponStateType
{
  public static readonly WakizashiStateType Aim = new WakizashiStateType("Aim");
  public static readonly WakizashiStateType Throw = new WakizashiStateType("Thrown");
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
    private WakizashiAimState _aimState;
    private WakizashiThrowState _throwState;
    private WakizashiReturnState _returnState;
    private WakizashiLodgedState _lodgedState;
    private WakizashiFallState _fallState;
    [HideInInspector] public Vector2 throwDirection;
    [HideInInspector] public float throwCooldownTimer;

    protected override void Awake()
    {
        weaponData = new WakizashiData();

        _idleState = new WakizashiIdleState(this);
        _attackState = new WakizashiAttackState(this);
        _parryState = new WakizashiParryState(this);
        _blockState = new WakizashiBlockState(this);
        _aimState = new WakizashiAimState(this);
        _throwState = new WakizashiThrowState(this);
        _returnState = new WakizashiReturnState(this);
        _lodgedState = new WakizashiLodgedState(this);
        _fallState = new WakizashiFallState(this);

        throwDirection = Vector2.zero;
        WakizashiData data = (WakizashiData)weaponData;
        throwCooldownTimer = data.throwCooldown;

        base.Awake();

        InputActions.Player.Throw_SlowTap.Enable();
        InputActions.Player.Throw_Hold.Enable();

        states.Add(WakizashiStateType.Idle, _idleState);
        states.Add(WakizashiStateType.Attack, _attackState);
        states.Add(WakizashiStateType.Parry, _parryState);
        states.Add(WakizashiStateType.Block, _blockState);
        states.Add(WakizashiStateType.Aim, _aimState);
        states.Add(WakizashiStateType.Throw, _throwState);
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
        _aimState.BindInput();
        _throwState.BindInput();
        _lodgedState.BindInput();
        _fallState.BindInput();

        InputActions.Player.Block.started += OnStartBlock;
    }

    private void OnDisable()
    {
        _idleState.UnbindInput();
        _attackState.UnbindInput();
        _blockState.UnbindInput();
        _aimState.UnbindInput();
        _throwState.UnbindInput();
        _lodgedState.UnbindInput();
        _fallState.UnbindInput();

        InputActions.Player.Block.started -= OnStartBlock;
    }

    private void OnStartBlock(InputAction.CallbackContext context)
    {
        if (!IsCurrentState(WakizashiStateType.Parry) && !IsCurrentState(WakizashiStateType.Block) && IsOnPlayer() && canBlock && blockCooldownTimer <= 0)
            SetState(states[WakizashiStateType.Parry]);
    }

    protected override void Update()
    {
        base.Update();

        // Handle Throw Cooldown
        if (!IsCurrentState(WakizashiStateType.Throw) && !IsCurrentState(WakizashiStateType.Return) && throwCooldownTimer > 0f) {
            throwCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (IsCurrentState(WakizashiStateType.Throw))
            _throwState.OnTriggerEnter2D(collider);
        else if (IsCurrentState(WakizashiStateType.Lodged))
            _lodgedState.OnTriggerEnter2D(collider);
        else if (IsCurrentState(WakizashiStateType.Fall))
            _fallState.OnTriggerEnter2D(collider);
    }


    // Called from Animation frames
    protected override void Attack(int isListeningForNextAttack)
    {
        if (IsCurrentState(WakizashiStateType.Attack)) {
            if (_currentState is WakizashiAttackState state) {
                state.Attack(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from Animation frames
    protected override void Upthrust(int isListeningForNextAttack)
    {
        if (IsCurrentState(WakizashiStateType.Attack)) {
            if (_currentState is WakizashiAttackState state) {
                state.Upthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from Animation frames
    protected override void Downthrust(int isListeningForNextAttack)
    {
        if (IsCurrentState(WakizashiStateType.Attack)) {
            if (_currentState is WakizashiAttackState state) {
                state.Downthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
            }
        }
    }

    // Called from animation frame
    protected override void EndAttack()
    {
        if (IsCurrentState(WakizashiStateType.Attack)) {
            if (_currentState is WakizashiAttackState state) {
                state.EndAttack();
            }
        }
    }

    // Called from animation frame
    protected override void OnNoNextAction()
    {
        if (IsCurrentState(WakizashiStateType.Attack)) {
            if (_currentState is WakizashiAttackState state) {
                state.OnNoNextAction();
            }
        }
    }

    public bool IsOnPlayer()
    {
        return !(IsCurrentState(WakizashiStateType.Throw)
              || IsCurrentState(WakizashiStateType.Lodged)
              || IsCurrentState(WakizashiStateType.Return));
    }

    public void UnlodgedFromEnemy()
    {
        if (IsCurrentState(WakizashiStateType.Lodged)) {
            SetState(states[WakizashiStateType.Fall]);
        }
    }
}
