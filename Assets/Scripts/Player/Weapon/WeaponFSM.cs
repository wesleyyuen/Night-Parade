using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFSM : MonoBehaviour
{
    public enum StateType
    {
        IdleState,
        AttackState,
        BlockState,
        ParryState
    }
    public WeaponData weaponData;
    public Rigidbody2D player {get; private set;}
    public PlayerMovement movement {get; private set;}
    public PlayerAbilityController abilityController {get; private set;}
    public PlayerAnimations animations {get; private set;}
    public Dictionary<StateType, WeaponState> states;
    WeaponState _currentState;
    [HideInInspector] public bool canAttack;
    [HideInInspector] public float attackCooldownTimer;
    [HideInInspector] public bool canBlock;
    [HideInInspector] public float blockCooldownTimer;
    [HideInInspector] public float currentBlockTimer; // Duration of the current block action
    public WeaponState previousState {get; private set;}

    protected virtual void Awake()
    {
        Transform playerGameObject = transform.parent;
        player = playerGameObject.GetComponent<Rigidbody2D>();
        movement = playerGameObject.GetComponent<PlayerMovement>();
        abilityController = playerGameObject.GetComponent<PlayerAbilityController>();
        animations = playerGameObject.GetComponent<PlayerAnimations>();
        states = new Dictionary<StateType, WeaponState>();
        canAttack = true;
        attackCooldownTimer = weaponData.attackCooldown;
        canBlock = true;
        blockCooldownTimer = weaponData.blockCooldown;
        currentBlockTimer = 0f;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (_currentState == null) return;

        // Handle Attacking Cooldown
        if (_currentState != states[StateType.AttackState] && attackCooldownTimer > 0) {
            attackCooldownTimer -= Time.deltaTime;
        }

        // Handle Blocking Duration and Cooldown
        if (_currentState != states[StateType.ParryState] && _currentState != states[StateType.BlockState]) {
            abilityController.RegenerateStamina();

            if (blockCooldownTimer > 0) {
                blockCooldownTimer -= Time.deltaTime;
            }
        }

        _currentState.Update(this);
    }

    protected virtual void FixedUpdate()
    {
        if (_currentState == null) return;

        _currentState.FixedUpdate(this);
    }

    public void SetState(WeaponState state)
    {
        if (_currentState != null) {
            _currentState.ExitState(this);
            previousState = _currentState;
        }

        _currentState = state;
        _currentState.EnterState(this);
    }

    public bool IsCurrentState(StateType state)
    {
        return _currentState == states[state];
    }

    public void EnablePlayerCombat(bool enable, float time = 0f)
    {
        if (canAttack == enable) return;

        if (time == 0)
            canAttack = enable;
        else
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => canAttack = e, time, enable, !enable));
    }

    public void EnablePlayerBlocking(bool enable, float time = 0)
    {
        if (canBlock == enable) return;
        
        if (time == 0)
            canBlock = enable;
        else
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => canBlock = e, time, enable, !enable));
    }

    // Called from Animation frames
    void StartListeningForNextAttackAndAttack()
    {
        if (_currentState == states[StateType.AttackState]) {
            WakizashiAttackState state = (WakizashiAttackState) _currentState;
            state.StartListeningForNextAttackAndAttack();
        }
    }

    // Called from Animation frames
    void Attack()
    {
        if (_currentState == states[StateType.AttackState]) {
            WakizashiAttackState state = (WakizashiAttackState) _currentState;
            state.Attack();
        }
    }

    // Called from animation frame
    void EndAttack ()
    {
        if (_currentState == states[StateType.AttackState]) {
            WakizashiAttackState state = (WakizashiAttackState) _currentState;
            state.EndAttack();
        }
    }

    public void Parry(bool isParryLeft, EnemyFSM fsm)
    {
        // Slow Time effect
        StartCoroutine( Utility.ChangeVariableAfterDelay<float>(e => Time.timeScale = e, 0.5f, 0.15f, 1f));

        Vector2 dir = new Vector2 (isParryLeft ? 1f : -1f, 0f);
        movement.ApplyKnockback(dir, weaponData.parryKnockback, 0.05f);
        fsm.ApplyKnockback(-dir, fsm.enemyData.knockBackOnParriedForce);
        fsm.SetState(fsm.states[EnemyFSM.StateType.StunnedState]);
    }

    public virtual void PlayWeaponMissSFX()
    {
        SoundManager.Instance.Play(weaponData.missSFX);
    } 

    public virtual void PlayWeaponHitSFX()
    {
        SoundManager.Instance.Play(weaponData.hitSFX);
    } 

    void OnDrawGizmosSelected()
    {
        if (weaponData != null) {
            // Show Attack Range
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.TransformPoint(weaponData.attackPoint), weaponData.attackRange);

            // Show Block Range
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.TransformPoint(weaponData.blockPoint), weaponData.blockRange);
    
            Gizmos.color = Color.white;
        }
    }
}
