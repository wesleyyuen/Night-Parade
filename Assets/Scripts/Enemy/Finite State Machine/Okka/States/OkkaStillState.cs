using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaStillState : EnemyState
{
    float _timer;
    public override void EnterState(EnemyFSM fsm)
    {
        _timer = 0;
        fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
    }

    public override void Update(EnemyFSM fsm)
    {
        _timer += Time.deltaTime;

        if (_timer >= fsm.enemyData.timeFrozenAfterDamagingPlayer) {
            // fsm.SetState(fsm.states[EnemyFSM.StateType.PatrolState]);
            if (fsm.IsInLineOfSight() || fsm.IsInAggroRange())
                fsm.SetState(fsm.states[EnemyFSM.StateType.AggroState]);
            else
                fsm.SetState(fsm.states[EnemyFSM.StateType.PatrolState]);
        }
    }

    public override void FixedUpdate(EnemyFSM fsm) {}

    public override void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}

    public override void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {
        GameObject go = collision.gameObject;
        if (go.layer == LayerMask.NameToLayer("Player")) {
            fsm.rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public override void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {
        GameObject go = collision.gameObject;
        if (go.layer == LayerMask.NameToLayer("Player")) {
            fsm.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public override void ExitState(EnemyFSM fsm)
    {
        fsm.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
