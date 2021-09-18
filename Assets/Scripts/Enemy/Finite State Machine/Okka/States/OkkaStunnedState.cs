using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaStunnedState : IEnemyState
{
    public float stunnedDuration;
    float _timer;
    public void EnterState(EnemyFSM fsm)
    {
        _timer = 0;
        fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
        fsm.LetRigidbodyMoveForSeconds(stunnedDuration);
    }

    public void Update(EnemyFSM fsm)
    {
        _timer += Time.deltaTime;

        if (_timer >= stunnedDuration) {
            if (fsm.IsInLineOfSight() || fsm.IsInAggroRange())
                fsm.SetState(fsm.states[EnemyFSM.StateType.AggroState]);
            else
                fsm.SetState(fsm.states[EnemyFSM.StateType.PatrolState]);
        }
    }

    public void FixedUpdate(EnemyFSM fsm) {}
    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}
    public void ExitState(EnemyFSM fsm) {}
}
