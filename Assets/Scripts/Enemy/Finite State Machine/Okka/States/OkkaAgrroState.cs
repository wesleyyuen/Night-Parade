using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaAgrroState : IEnemyState
{
    float _delayTimer;
    public void EnterState(EnemyFSM fsm)
    {
        _delayTimer = 0;

        fsm.GFX.SetAnimatorBoolean("IsPatrolling", true);
        fsm.GFX.SetAnimatorSpeed(fsm.enemyData.aggroMovementSpeed / fsm.enemyData.patrolSpeed * 0.75f);

        if (fsm.previousState == fsm.states[EnemyFSM.StateType.PatrolState])
            fsm.GFX.FlashExclaimationMark();
    }

    public void Update(EnemyFSM fsm)
    {
        // check Line of Sight
        bool inLineOfSight = fsm.IsInLineOfSight();
        // Always face player when aggro
        fsm.GFX.FaceTowardsPlayer(0.0f);

        // Drop aggro after a certain delay period without LOS
        if (!inLineOfSight) {
            _delayTimer += Time.deltaTime;

            if (_delayTimer >= fsm.enemyData.lineOfSightBreakDelay)
                fsm.SetState(fsm.states[EnemyFSM.StateType.LostLOSState]);

        } else {
            _delayTimer = 0;
        }
    }

    public void FixedUpdate(EnemyFSM fsm)
    {
        if (!fsm.GFX.IsTurning()) {
            if (Vector2.Distance(fsm.player.attachedRigidbody.position, fsm.rb.position) <= fsm.enemyData.attackDistance && fsm.IsInLineOfSight())
                fsm.SetState(fsm.states[EnemyFSM.StateType.AttackState]);
            else
                MoveTowardsPlayer(ref fsm);
        }
    }

    void MoveTowardsPlayer(ref EnemyFSM fsm)
    {
        Vector2 target = new Vector2 (fsm.player.bounds.center.x, fsm.rb.position.y);
        
        Vector2 direction = (target - (Vector2) fsm.transform.position).normalized;
        fsm.rb.velocity = new Vector2(direction.x * fsm.enemyData.aggroMovementSpeed , fsm.rb.velocity.y);
    }

    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}
    
    public void ExitState(EnemyFSM fsm)
    {
        fsm.GFX.SetAnimatorSpeed(1f);
    }
}
