using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public interface IEnemyState : IState, IUpdateLoop, ICollision2D
{

}

public class EnemyFSM : MonoBehaviour
{
    public enum StateType
    {
        PatrolState,
        AggroState,
        AttackState,
        LostLOSState,
        StillState,
        StunnedState,
        DamagedState,
        DeathState
    }
    public EnemyData enemyData;
    public Rigidbody2D rb {get; private set;}
    public Collider2D col {get; private set;}
    public EnemyGFX GFX {get; private set;}
    public Collider2D player {get; private set;}
    public Dictionary<StateType, IEnemyState> states;
    IEnemyState _currentState;
    public IEnemyState previousState {get; private set;}
    bool _isLetRBMove;
    bool _isDead;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        GFX = GetComponent<EnemyGFX>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
        states = new Dictionary<StateType, IEnemyState>();
        _isLetRBMove = false;
        _isDead = false;
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

    protected virtual void Update()
    {
        if (_currentState == null || player == null || _isLetRBMove) return;

        _currentState.Update();
    }

    protected virtual void FixedUpdate()
    {
        if (_currentState == null || player == null || _isLetRBMove) return;

        _currentState.FixedUpdate();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentState == null || player == null) return;
        
        _currentState.OnCollisionEnter2D(collision);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (_currentState == null || player == null) return;

        GameObject go = collision.gameObject;
        if (go.layer == LayerMask.NameToLayer("Player")) {
            SetState(states[StateType.StillState]);
            go.GetComponent<PlayerHealth>().HandleDamage(enemyData.damageAmount, rb.position);
        }

        _currentState.OnCollisionStay2D(collision);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (_currentState == null || player == null) return;

        _currentState.OnCollisionExit2D(collision);
    }

    public void SetState(IEnemyState state)
    {
        if (state == null) return;
        
        if (_currentState != null) {
            _currentState.ExitState();
            previousState = _currentState;
        }

        _currentState = state;
        _currentState.EnterState();
    }

    public bool IsCurrentState(StateType state)
    {
        return _currentState == states[state];
    }

    public void StunForSeconds(float duration)
    {
        OkkaStunnedState stunnedState = (OkkaStunnedState) states[StateType.StunnedState];
        stunnedState.stunnedDuration = duration;
        SetState(states[StateType.StunnedState]);
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
        if (enemyToPlayerVector.magnitude > enemyData.aggroDistance) return false;

        // Check Line of sight with raycast2d
        int layerMasks = 1 << LayerMask.NameToLayer("Player");
        RaycastHit2D los = Physics2D.Raycast(lineOfSightOrigin, enemyToPlayerVector, enemyData.lineOfSightDistance, layerMasks);
        if (los) return los.collider.CompareTag("Player");

        return false;
    }

    public void LetRigidbodyMoveForSeconds(float time)
    {
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _isLetRBMove = e, time, true, false));
    }

    public void ApplyForce(Vector2 dir, float force, float time = 0)
    {
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<float>(e => rb.drag = e, time == 0 ? 0.1f : time, force * 0.1f, 1f));
        LetRigidbodyMoveForSeconds(time == 0 ? 0.1f : time);
        
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    }

    public virtual void TakeDamage(float damage, Vector2 dir)
    {
        if (_isDead || player == null || _currentState == null) return;

        enemyData.currentHealth -= damage;

        // Death
        if (enemyData.currentHealth <= 0) {
            _isDead = true;
            SetState(states[StateType.DeathState]);
        } else {
            if (Constant.ATTACK_STUNS_ENEMY) {
                // Can use StunForSeconds
                SetState(states[StateType.DamagedState]);
            } else {
                // Play Damaged Effect
                GFX.PlayDamagedEffect();

                // Apply Knock back
                if (!IsCurrentState(StateType.AttackState)) {
                    ApplyForce(-dir, enemyData.knockBackOnTakingDamageForce, enemyData.timeFrozenAfterTakingDamage);
                }
            }
        }
    }

    public bool IsAttacking()
    {
        return _currentState == states[StateType.AggroState];
    }

    public bool IsDead()
    {
        return _isDead;
    }
}
