using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public interface IEnemyState : IState, IUpdateLoop, ICollision2D
{
}

public class EnemyStateType : StateType
{
    public static readonly EnemyStateType Aggro = new EnemyStateType("Aggro");
    public static readonly EnemyStateType Stunned = new EnemyStateType("Stunned");
    public static readonly EnemyStateType Death = new EnemyStateType("Death");

    protected EnemyStateType(string value) : base(value) {}
}

public class RegularEnemyStateType : EnemyStateType
{
    public static readonly EnemyStateType Patrol = new RegularEnemyStateType("Patrol");

    protected RegularEnemyStateType(string value) : base(value) {}
}

public class BossEnemyStateType : EnemyStateType
{
    protected BossEnemyStateType(string value) : base(value) {}
}


public abstract class EnemyFSM : MonoBehaviour, IDamageable
{
    public EnemyData enemyData;
    public Rigidbody2D rb {get; private set;}
    public Collider2D col {get; private set;}
    public EnemyGFX GFX {get; private set;}
    public Collider2D player {get; private set;}
    public Dictionary<StateType, IEnemyState> states;
    [HideInInspector] public string stateParameter;
    private IEnemyState _currentState, _previousState;
    private float _currentHealth;
    private bool _isLetRBMove;
    private bool _isDead;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        GFX = GetComponent<EnemyGFX>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        states = new Dictionary<StateType, IEnemyState>();
    }

    protected virtual void Start()
    {
        // Don't spawn enemies if before timestamp
        SceneData data = SaveManager.Instance.GetSceneData(GameMaster.Instance.currentScene);
        Dictionary<string, float> timestamps = data.EnemySpawnTimestamps;
        if (timestamps.ContainsKey(gameObject.name)) {
            if (Time.time < timestamps[gameObject.name])
                Destroy(gameObject);
        } else {
            SaveManager.Instance.UpdateSpawnTimestamp(gameObject.name, 0f);
        }
    }

    protected virtual void OnEnable()
    {
        _currentHealth = enemyData.maxHealth;
        stateParameter = "";
        _isLetRBMove = false;
        _isDead = false;
    }

    protected virtual void Update()
    {
        if (player == null || _isLetRBMove) return;

        _currentState?.Update();
    }

    protected virtual void FixedUpdate()
    {
        if (player == null || _isLetRBMove) return;

        _currentState?.FixedUpdate();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (player == null) return;
        
        _currentState?.OnCollisionEnter2D(collision);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (player == null) return;

        GameObject go = collision.gameObject;
        if (!_isDead && go.layer == LayerMask.NameToLayer("Player")) {
            StunForSeconds(enemyData.timeStunnedAfterDamagingPlayer);
            go.GetComponent<PlayerHealthMO>().HandleDamage(enemyData.damageAmount, rb.position);
        }

        _currentState?.OnCollisionStay2D(collision);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (player == null) return;

        _currentState?.OnCollisionExit2D(collision);
    }

    public void SetState(IEnemyState state)
    {
        if (state == null || _currentState == state) return;
        
        _currentState?.ExitState();
        _previousState = _currentState;

        Debug.Log(state);

        _currentState = state;
        _currentState?.EnterState();
    }

    public bool IsCurrentState(StateType state)
    {
        if (states.TryGetValue(state, out IEnemyState iState)) {
            return _currentState == iState;
        }

        return false;
    }

    public bool IsPreviousState(StateType state)
    {
        if (states.TryGetValue(state, out IEnemyState iState)) {
            return _currentState == iState;
        }

        return false;
    }

    public void StunForSeconds(float duration)
    {
        if (IsCurrentState(EnemyStateType.Stunned)) return;

        stateParameter = duration.ToString();
        SetState(states[EnemyStateType.Stunned]);
    }   

    public virtual bool IsInLineOfSight()
    {
        // Check Distance to player
        Vector3 lineOfSightOrigin = transform.position + (Vector3) enemyData.lineOfSightOriginOffset;
        Vector2 enemyToPlayerVector = player.bounds.ClosestPoint(lineOfSightOrigin) - lineOfSightOrigin;
        if (enemyToPlayerVector.magnitude > enemyData.lineOfSightDistance) return false;

        // Check Line of sight angle
        float angle = Vector2.Angle(enemyToPlayerVector, GFX.GetEnemyScale().x * transform.right);
        if (angle > enemyData.lineOfSightAngle) return false;

        // Check Line of sight with raycast2d
        int layerMasks = 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D los = Physics2D.Raycast(lineOfSightOrigin, enemyToPlayerVector, enemyData.lineOfSightDistance, layerMasks);
        if (los) return los.collider.CompareTag("Player");

        return false;
    }

    public virtual bool IsInAggroRange()
    {
        // Check Distance to player
        Vector3 lineOfSightOrigin = transform.position + (Vector3) enemyData.lineOfSightOriginOffset;
        Vector2 enemyToPlayerVector = player.bounds.ClosestPoint(lineOfSightOrigin) - lineOfSightOrigin;
        if (enemyToPlayerVector.magnitude > enemyData.noLOSAggroDistance) return false;

        // Check Line of sight with raycast2d
        int layerMasks = 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D los = Physics2D.Raycast(lineOfSightOrigin, enemyToPlayerVector, enemyData.lineOfSightDistance, layerMasks);
        if (los) return los.collider.CompareTag("Player");

        return false;
    }

    public void LetRigidbodyMoveForSeconds(float time)
    {
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _isLetRBMove = e, time, true, false).CancelWith(gameObject));
    }

    public virtual void ApplyForce(Vector2 dir, float force, float time = 0)
    {
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<float>(e => rb.drag = e, time == 0 ? 0.1f : time, force * 0.1f, 1f).CancelWith(gameObject));
        LetRigidbodyMoveForSeconds(time == 0 ? 0.1f : time);
        
        rb.velocity = Vector2.zero;
        rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    }

    public virtual bool TakeDamage(float damage, Vector2 dir)
    {
        if (_isDead || player == null || _currentState == null) return false;

        _currentHealth -= damage;

        // Death
        if (_currentHealth <= 0) {
            _isDead = true;
            stateParameter = dir.x.ToString();
            SetState(states[EnemyStateType.Death]);
        } else {
            // Play Damaged Effect
            GFX.PlayDamagedEffect();

            if (Constant.ATTACK_STUNS_ENEMY) {
                StunForSeconds(enemyData.timeStunnedAfterTakingDamage);

                // Apply Knock back
                ApplyForce(dir, enemyData.knockBackOnTakingDamageForce, enemyData.timeStunnedAfterTakingDamage);
            } else {
                // Apply Knock back
                if (!IsAttacking()/* && !IsCurrentState(EnemyStateType.Stunned)*/) {
                    ApplyForce(dir, enemyData.knockBackOnTakingDamageForce, enemyData.timeStunnedAfterTakingDamage);
                }
            }
        }

        return true;
    }

    public abstract bool IsAttacking();

    public bool IsDead()
    {
        return _isDead;
    }
}
