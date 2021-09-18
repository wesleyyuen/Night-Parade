using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaStillState : IEnemyState
{
    float _timer;
    public void EnterState(EnemyFSM fsm)
    {
        _timer = 0;
        fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
    }

    public void Update(EnemyFSM fsm)
    {
        _timer += Time.deltaTime;

        if (_timer >= fsm.enemyData.timeFrozenAfterDamagingPlayer) {
            if ((fsm.IsInLineOfSight() || fsm.IsInAggroRange())
                && fsm.states[EnemyFSM.StateType.AggroState] != null)
                fsm.SetState(fsm.states[EnemyFSM.StateType.AggroState]);
            else
                fsm.SetState(fsm.states[EnemyFSM.StateType.PatrolState]);
        }
    }

    public void FixedUpdate(EnemyFSM fsm) {}

    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}

    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {
        GameObject go = collision.gameObject;
        if (go.layer == LayerMask.NameToLayer("Player")) {
            fsm.rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {
        GameObject go = collision.gameObject;
        if (go.layer == LayerMask.NameToLayer("Player")) {
            fsm.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void ExitState(EnemyFSM fsm)
    {
        fsm.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
