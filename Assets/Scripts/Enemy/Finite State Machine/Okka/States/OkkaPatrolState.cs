using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaPatrolState : IEnemyState
{
    const float _ORIGIN_OFFSET = 0.1f;
    // TODO: temp fix to repeatedly turning in BUILD ONLY
    const float _TURN_COOLDOWN = 1f;
    float _turnCooldownTimer;
    int layerMasks;

    public void EnterState(EnemyFSM fsm)
    {
        layerMasks = LayerMask.GetMask("Ground");
        _turnCooldownTimer = 0f;
        fsm.GFX.SetAnimatorBoolean("IsPatrolling", true);
    }

    public void Update(EnemyFSM fsm)
    {
        if (fsm.IsInAggroRange() || fsm.IsInLineOfSight())
            fsm.SetState(fsm.states[EnemyFSM.StateType.AggroState]);

        if (_turnCooldownTimer >= 0f) {
            _turnCooldownTimer -= Time.deltaTime;
        }
    }

    // TODO: Can this be optimized further?
    public void FixedUpdate(EnemyFSM fsm)
    {
        Vector2 groundDetectionPoint = new Vector2 (fsm.GFX.GetEnemyScale().x == 1f ? fsm.col.bounds.max.x + _ORIGIN_OFFSET: fsm.col.bounds.min.x - _ORIGIN_OFFSET, fsm.col.bounds.center.y);

        RaycastHit2D groundHit = Physics2D.Raycast(groundDetectionPoint, -fsm.transform.up, fsm.col.bounds.size.y, LayerMask.GetMask("Ground"));
        RaycastHit2D wallHit = Physics2D.Raycast(groundDetectionPoint, -fsm.transform.up, fsm.col.bounds.size.y, LayerMask.GetMask("Wall"));
        // Debug.DrawRay(groundDetectionPoint, -fsm.transform.up * fsm.col.bounds.size.y, Color.green);

        if ((!groundHit || (wallHit && wallHit.collider.name == "Others")) && _turnCooldownTimer <= 0f) {
            _turnCooldownTimer = _TURN_COOLDOWN;
            fsm.GFX.TurnAround(true);
        }

        // Move forward
        fsm.rb.velocity = new Vector2(fsm.GFX.GetEnemyScale().x * fsm.enemyData.patrolSpeed , fsm.rb.velocity.y);
    }

    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}
    public void ExitState(EnemyFSM fsm) {}
}
