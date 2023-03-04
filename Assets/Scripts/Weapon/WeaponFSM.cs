using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public interface IWeaponState : IState, IUpdateLoop
{
    
}

public class WeaponStateType : StateType
{
    public static readonly WeaponStateType Idle = new WeaponStateType("Idle");
    public static readonly WeaponStateType Attack = new WeaponStateType("Attack");
    public static readonly WeaponStateType Block = new WeaponStateType("Block");
    public static readonly WeaponStateType Parry = new WeaponStateType("Parry");

    protected WeaponStateType(string value) : base(value) {}
}

public class WeaponFSM : MonoBehaviour
{
    public WeaponData weaponData;
    // public AudioEventSO hitSFX, missSFX;
    public Rigidbody2D player {get; private set;}
    public Dictionary<WeaponStateType, IWeaponState> states;
    protected IWeaponState _currentState;
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

        states = new Dictionary<WeaponStateType, IWeaponState>();
        canAttack = true;
        attackCooldownTimer = weaponData.attackCooldown;
        canBlock = true;
        hasBlocked = false;
        blockCooldownTimer = weaponData.blockCooldown;
        currentBlockTimer = 0f;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        _currentState?.Update();

        // Handle Attacking Cooldown
        if (attackCooldownTimer > 0) {
            attackCooldownTimer -= Time.deltaTime;
        }

        // Handle Blocking Duration and Cooldown
        if (_currentState != states[WeaponStateType.Parry] && _currentState != states[WeaponStateType.Block]) {
            if (blockCooldownTimer > 0) {
                blockCooldownTimer -= Time.deltaTime;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (player == null) return;

        _currentState?.FixedUpdate();
    }

    public void SetState(IWeaponState state)
    {
        if (state == null || _currentState == state) return;

        _currentState?.ExitState();
        previousState = _currentState;

        _currentState = state;
        _currentState?.EnterState();
    }


    public bool IsCurrentState(WeaponStateType state)
    {
        if (states.TryGetValue(state, out IWeaponState iState)) {
            return _currentState == iState;
        }

        return false;
    }
    public IWeaponState GetStateByType(WeaponStateType state)
    {
        if (states.TryGetValue(state, out IWeaponState iState)) {
            return iState;
        }

        return null;
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
    protected virtual void Attack(int isListeningForNextAttack)
    {
    }

    // Called from Animation frames
    protected virtual void Upthrust(int isListeningForNextAttack)
    {
    }

    // Called from Animation frames
    protected virtual void Downthrust(int isListeningForNextAttack)
    {
    }

    // Called from animation frame
    protected virtual void EndAttack()
    {
    }

    // Called from animation frame
    protected virtual void OnNoNextAction()
    {
    }

    public bool IsAttacking()
    {
        return IsCurrentState(WeaponStateType.Attack);
    }

    // Audio
    public void PlayWeaponMissSFX()
    {
        weaponData.missSFX.Play();
    } 

    public void PlayWeaponHitSFX()
    {
        weaponData.hitSFX.Play();
    }

    private void OnDrawGizmosSelected()
    {
        if (weaponData != null) {
            // Show Attack Range
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.TransformPoint(weaponData.attackPoint), weaponData.attackRange);

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
