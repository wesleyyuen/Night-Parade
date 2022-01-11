using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OkkaPatrolState : IEnemyState
{
    private OkkaFSM _fsm;
    private const float _DETECTION_OFFSET = 0.1f;
    // TODO: temp fix to repeatedly turning in BUILD ONLY
    private const float _TURN_COOLDOWN = 1f;
    private Vector2 _origin;
    private float _turnCooldownTimer;

    public OkkaPatrolState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }

    public void EnterState()
    {
        _origin = _fsm.rb.position;
        _turnCooldownTimer = 0f;
        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", true);
    }

    public void Update()
    {
        if (_fsm.IsInAggroRange() || _fsm.IsInLineOfSight()) {
            _fsm.SetState(_fsm.states[EnemyFSM.StateType.AggroState]);
        }

        if (_turnCooldownTimer >= 0f) {
            _turnCooldownTimer -= Time.deltaTime;
        }
    }

    // TODO: Can this be optimized further?
    public void FixedUpdate()
    {
        Vector2 groundDetectionPoint = new Vector2 (_fsm.GFX.GetEnemyScale().x == 1f ? _fsm.col.bounds.max.x + _DETECTION_OFFSET: _fsm.col.bounds.min.x - _DETECTION_OFFSET, _fsm.col.bounds.center.y);

        RaycastHit2D groundHit = Physics2D.Raycast(groundDetectionPoint, -_fsm.transform.up, _fsm.col.bounds.size.y, LayerMask.GetMask("Ground"));
        RaycastHit2D wallHit = Physics2D.Raycast(groundDetectionPoint, -_fsm.transform.up, _fsm.col.bounds.size.y, LayerMask.GetMask("Wall"));
        // Debug.DrawRay(groundDetectionPoint, -_fsm.transform.up * _fsm.col.bounds.size.y, Color.green);

        bool hasReachedPatrolMaxDist = _fsm.rb.position.x < (_origin.x - _fsm.enemyData.patrolDistance) || _fsm.rb.position.x > (_origin.x + _fsm.enemyData.patrolDistance);
        bool hasNoGroundInFront = !groundHit;
        bool hasWallInFront = wallHit && wallHit.collider.name == "Others";
        bool shouldTurnAround = hasReachedPatrolMaxDist || hasNoGroundInFront || hasWallInFront;

        if (shouldTurnAround && _turnCooldownTimer <= 0f) {
            _turnCooldownTimer = _TURN_COOLDOWN;
            _fsm.GFX.TurnAround(true);
        }

        // Move forward
        _fsm.rb.velocity = new Vector2(_fsm.GFX.GetEnemyScale().x * _fsm.enemyData.patrolSpeed , _fsm.rb.velocity.y);
    }

    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}
    public void ExitState() {}
}
