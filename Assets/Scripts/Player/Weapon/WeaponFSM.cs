using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public interface IWeaponState : IState, IUpdateLoop
{
}

public class WeaponFSM : MonoBehaviour
{
    public enum StateType
    {
        IdleState,
        AttackState,
        BlockState,
        ParryState,
        ThrownState,
        ReturnState,
        LodgedState,
        FallState
    }
    public WeaponData weaponData;
    public Rigidbody2D player {get; private set;}
    public Dictionary<StateType, IWeaponState> states;
    public InputMaster InputActions;
    private IWeaponState _currentState;
    [HideInInspector] public GameObject onibi;
    [HideInInspector] public bool canAttack;
    [HideInInspector] public float attackCooldownTimer;
    [HideInInspector] public bool canBlock;
    [HideInInspector] public bool hasBlocked;
    [HideInInspector] public bool hasParried;
    [HideInInspector] public float blockCooldownTimer;
    [HideInInspector] public float currentBlockTimer; // Duration of the current block action
    public IWeaponState previousState {get; private set;}

    protected virtual void Awake()
    {
        Transform playerGameObject = transform.parent;
        player = playerGameObject.GetComponent<Rigidbody2D>();
        foreach (Transform obj in playerGameObject.parent.transform) {
            if (obj.tag == "Onibi") onibi = obj.gameObject;
        }

        states = new Dictionary<StateType, IWeaponState>();
        canAttack = true;
        attackCooldownTimer = weaponData.attackCooldown;
        canBlock = true;
        hasBlocked = false;
        blockCooldownTimer = weaponData.blockCooldown;
        currentBlockTimer = 0f;

        // Handle Actions
        InputActions = new InputMaster();
        InputActions.Player.Attack.Enable();
        InputActions.Player.Block.Enable();
        InputActions.Player.Throw.Enable();
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
            // abilityController.RegenerateStamina();

            if (blockCooldownTimer > 0) {
                blockCooldownTimer -= Time.deltaTime;
            }
        }

        _currentState.Update();
    }

    protected virtual void FixedUpdate()
    {
        if (_currentState == null) return;

        _currentState.FixedUpdate();
    }

    public bool IsCurrentState(StateType type)
    {
        return _currentState == states[type];
    }

    public IWeaponState GetStateByType(StateType type)
    {
        return states[type];
    }

    public void SetState(IWeaponState state)
    {
        if (_currentState != null) {
            _currentState.ExitState();
            previousState = _currentState;
        }

        _currentState = state;
        _currentState.EnterState();
    }

    public void SetStateAfterDelay(StateType state, float delay)
    {
        Timing.RunCoroutine(_SetStateAfterDelayCoroutine(state, delay));
    }

    IEnumerator<float> _SetStateAfterDelayCoroutine(StateType state, float delay)
    {
        // Only Set State if no one set another state before the delay ends
        IState curr = _currentState;
        yield return Timing.WaitForSeconds(delay);
        if (_currentState == curr)
            SetState(states[state]);
    }

    public void EnablePlayerCombat(bool enable, float time = 0f)
    {
        if (canAttack == enable) return;

        if (time == 0)
            canAttack = enable;
        else
            Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => canAttack = e, time, enable, !enable));
    }

    public void EnablePlayerBlocking(bool enable, float time = 0)
    {
        if (canBlock == enable) return;
        
        if (time == 0)
            canBlock = enable;
        else
            Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => canBlock = e, time, enable, !enable));
    }

    // Called from Animation frames
    private void Attack(int isListeningForNextAttack)
    {
        if (_currentState == states[StateType.AttackState]) {
            WakizashiAttackState state = (WakizashiAttackState) _currentState;
            state.Attack(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
        }
    }

    // Called from Animation frames
    private void Upthrust(int isListeningForNextAttack)
    {
        if (_currentState == states[StateType.AttackState]) {
            WakizashiAttackState state = (WakizashiAttackState) _currentState;
            state.Upthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
        }
    }

    // Called from Animation frames
    private void Downthrust(int isListeningForNextAttack)
    {
        if (_currentState == states[StateType.AttackState]) {
            WakizashiAttackState state = (WakizashiAttackState) _currentState;
            state.Downthrust(Constant.HAS_TIMED_COMBO ? isListeningForNextAttack != 0 : true);
        }
    }

    // Called from animation frame
    private void EndAttack()
    {
        if (_currentState == states[StateType.AttackState]) {
            WakizashiAttackState state = (WakizashiAttackState) _currentState;
            state.EndAttack();
        }
    }

    public bool IsOnPlayer()
    {
        return !(IsCurrentState(StateType.ThrownState) || IsCurrentState(StateType.LodgedState));
    }

    public void UnlodgedFromEnemy()
    {
        if (IsCurrentState(StateType.LodgedState)) {
            SetState(states[StateType.FallState]);
        }
    }

    // Onibi Interactions
    // public virtual IEnumerator<float> _MergeWithOnibi(float waitDuration)
    // {
    //     onibi.GetComponent<OnibiMovement>().StartMerging(GetComponent<SpriteRenderer>().bounds.center);
    //     Utility.FadeGameObjectRecursively(onibi, 1f, 0f, waitDuration);
    //     yield return Timing.WaitForSeconds(waitDuration);
    //     GetComponent<SpriteRenderer>().material = _glowMaterial;
    // }

    // Audio
    public virtual void PlayWeaponMissSFX()
    {
        SoundManager.Instance.Play(weaponData.missSFX);
    } 

    public virtual void PlayWeaponHitSFX()
    {
        SoundManager.Instance.Play(weaponData.hitSFX);
    }

    private void OnDrawGizmosSelected()
    {
        if (weaponData != null) {
            // Show Attack Range
            Gizmos.color = Color.red;
            // Gizmos.DrawWireCube(transform.TransformPoint(weaponData.attackPoint), weaponData.attackRange);

            // Show Upthrust Range
            // Gizmos.DrawWireCube(transform.TransformPoint(weaponData.upthrustPoint), weaponData.upthrustRange);

            // // Show Downthrust Range
            // Gizmos.DrawWireCube(transform.TransformPoint(weaponData.downthrustPoint), weaponData.downthrustRange);

            // // Show Block Range
            // Gizmos.color = Color.blue;
            // Gizmos.DrawWireCube(transform.TransformPoint(weaponData.blockPoint), weaponData.blockRange);

            // Show Collider
            Gizmos.DrawWireCube(transform.position, new Vector2(1f, 1f));
    
            Gizmos.color = Color.white;
        }
    }
}
