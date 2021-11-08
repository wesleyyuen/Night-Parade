using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaPatrolState : IEnemyState
{
    float _patrolOriginOffset = 0.1f;
    float _raycastDistance = 0.5f;
    int layerMasks;

    public void EnterState(EnemyFSM fsm)
    {
        layerMasks = LayerMask.GetMask("Ground");
        fsm.GFX.SetAnimatorBoolean("IsPatrolling", true);
    }

    public void Update(EnemyFSM fsm)
    {
        if (fsm.IsInAggroRange() || fsm.IsInLineOfSight())
            fsm.SetState(fsm.states[EnemyFSM.StateType.AggroState]);
    }

    // TODO: Can this be optimized further?
    public void FixedUpdate(EnemyFSM fsm)
    {
        Vector2 scale = fsm.GFX.GetEnemyScale();
        Vector2 target = fsm.transform.position;
        Vector2 groundDetectionPoint = new Vector2 (scale.x == 1f ? fsm.col.bounds.max.x + _patrolOriginOffset: fsm.col.bounds.min.x - _patrolOriginOffset, fsm.col.bounds.min.y);
        
        RaycastHit2D[] hits = Physics2D.RaycastAll(groundDetectionPoint, -fsm.transform.up, _raycastDistance, layerMasks);
        // Debug.DrawRay(groundDetectionPoint, -fsm.transform.up * _raycastDistance, Color.green);

        bool shouldTurnAround = hits.Length == 0;
        foreach (RaycastHit2D hit in hits) {
            shouldTurnAround = hit.collider.name == "Others";
            if (shouldTurnAround) break;
        }

        if (shouldTurnAround) {
            fsm.GFX.TurnAround(true);
        }
        
        target = new Vector2(groundDetectionPoint.x, fsm.rb.position.y);

        // Move towards target location
        Vector2 direction = (target - fsm.rb.position).normalized;
        fsm.rb.velocity = new Vector2(direction.x * fsm.enemyData.patrolSpeed , fsm.rb.velocity.y);
    }

    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}
    public void ExitState(EnemyFSM fsm) {}
}
