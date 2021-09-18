using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaPatrolState : IEnemyState
{
    float _patrolOriginOffset = 0.1f;
    public void EnterState(EnemyFSM fsm)
    {
        fsm.GFX.SetAnimatorBoolean("IsPatrolling", true);
    }

    public void Update(EnemyFSM fsm)
    {
        if (fsm.IsInAggroRange() || fsm.IsInLineOfSight())
            fsm.SetState(fsm.states[EnemyFSM.StateType.AggroState]);
    }

    public void FixedUpdate(EnemyFSM fsm)
    {
        Vector2 target = fsm.transform.position;
        Vector2 groundDetectionPoint = new Vector2 (fsm.transform.localScale.x == 1f ? fsm.col.bounds.max.x + _patrolOriginOffset: fsm.col.bounds.min.x - _patrolOriginOffset, fsm.col.bounds.center.y);

        RaycastHit2D hit = Physics2D.Raycast(groundDetectionPoint, -fsm.transform.up, fsm.col.bounds.size.y, LayerMask.GetMask("Ground"));
        Debug.DrawRay(groundDetectionPoint, -fsm.transform.up * fsm.col.bounds.size.y, Color.green);

        if (!hit || hit.collider.CompareTag("GroundEdges")) {
            fsm.GFX.TurnAround(true);
            groundDetectionPoint = new Vector2 (fsm.transform.localScale.x == 1f ? fsm.col.bounds.min.x : fsm.col.bounds.max.x, fsm.col.bounds.center.y);
        }
        target = new Vector2 (groundDetectionPoint.x, fsm.rb.position.y);

        // Move towards target location
        Vector2 direction = (target - fsm.rb.position).normalized;
        fsm.rb.velocity = new Vector2(direction.x * fsm.enemyData.patrolSpeed , fsm.rb.velocity.y);
    }

    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}
    public void ExitState(EnemyFSM fsm) {}
}
