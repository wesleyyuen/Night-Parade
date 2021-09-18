using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider2D>();
        states = new Dictionary<StateType, IEnemyState>();
        _isLetRBMove = false;
        _isDead = false;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (_currentState == null || player == null || _isLetRBMove) return;

        _currentState.Update(this);
    }

    protected virtual void FixedUpdate()
    {
        if (_currentState == null || player == null || _isLetRBMove) return;

        _currentState.FixedUpdate(this);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (_currentState == null || player == null) return;
        
        _currentState.OnCollisionEnter2D(this, collision);
    }

    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        if (_currentState == null || player == null) return;

        GameObject go = collision.gameObject;
        if (go.layer == LayerMask.NameToLayer("Player")) {
            SetState(states[StateType.StillState]);
            go.GetComponent<PlayerHealth>().HandleDamage(enemyData.damageAmount, rb.position);
        }

        _currentState.OnCollisionStay2D(this, collision);
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (_currentState == null || player == null) return;

        _currentState.OnCollisionExit2D(this, collision);
    }

    public void SetState(IEnemyState state)
    {
        if (state == null) return;
        
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

    public void StunForSeconds(float duration)
    {
        OkkaStunnedState stunnedState = (OkkaStunnedState) states[StateType.StunnedState];
        stunnedState.stunnedDuration = duration;
        SetState(states[StateType.StunnedState]);
    }   

    public bool IsInLineOfSight()
    {
        // Check Distance to player
        Vector3 lineOfSightOrigin = transform.position + (Vector3) enemyData.lineOfSightOriginOffset;
        Vector2 enemyToPlayerVector = player.bounds.ClosestPoint(lineOfSightOrigin) - lineOfSightOrigin;
        if (enemyToPlayerVector.magnitude > enemyData.lineOfSightDistance) return false;

        // Check Line of sight angle
        float angle = Vector2.Angle(enemyToPlayerVector, transform.localScale.x * transform.right);
        if (angle > enemyData.lineOfSightAngle) return false;

        // Check Line of sight with raycast2d
        int layerMasks = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D los = Physics2D.Raycast(lineOfSightOrigin, enemyToPlayerVector, enemyData.lineOfSightDistance, layerMasks);
        if (los) return los.collider.CompareTag("Player");

        return false;
    }

    public bool IsInAggroRange()
    {
        // Check Distance to player
        Vector3 lineOfSightOrigin = transform.position + (Vector3) enemyData.lineOfSightOriginOffset;
        Vector2 enemyToPlayerVector = player.bounds.ClosestPoint(lineOfSightOrigin) - lineOfSightOrigin;
        if (enemyToPlayerVector.magnitude > enemyData.aggroDistance) return false;

        // Check Line of sight with raycast2d
        int layerMasks = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D los = Physics2D.Raycast(lineOfSightOrigin, enemyToPlayerVector, enemyData.lineOfSightDistance, layerMasks);
        if (los) return los.collider.CompareTag("Player");

        return false;
    }

    public void LetRigidbodyMoveForSeconds(float time)
    {
        StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _isLetRBMove = e, time, true, false));
    }

    public void ApplyForce(Vector2 dir, float force, float time = 0)
    {
        StartCoroutine(Utility.ChangeVariableAfterDelay<float>(e => rb.drag = e, time == 0 ? 0.1f : time, force * 0.1f, 1f));
        LetRigidbodyMoveForSeconds(time == 0 ? 0.1f : time);
        
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    }

    public virtual void TakeDamage(float damage)
    {
        if (_isDead || player == null || _currentState == null) return;

        enemyData.currentHealth -= damage;

        // Death
        if (enemyData.currentHealth <= 0) {
            _isDead = true;
            SetState(states[StateType.DeathState]);
        } else {
            if (Constant.stunEnemyAfterAttack) {
                // Can use StunForSeconds
                SetState(states[StateType.DamagedState]);
            } else {
                // Play Damaged Effect
                GFX.PlayDamagedEffect();

                // Apply Knock back
                if (!IsCurrentState(StateType.AttackState)) {
                    bool playerOnLeft = rb.position.x > player.transform.position.x;
                    ApplyForce(playerOnLeft ? Vector2.right : Vector2.left, enemyData.knockBackOnTakingDamageForce, enemyData.timeFrozenAfterTakingDamage);
                }
            }
        }
    }

    public void Die()
    {
        GetComponent<EnemyDrop>().SpawnDrops();
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return _isDead;
    }
}
