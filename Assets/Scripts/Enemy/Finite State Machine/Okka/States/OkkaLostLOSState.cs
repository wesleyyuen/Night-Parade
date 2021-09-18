using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaLostLOSState : IEnemyState
{
    float _timer;
    public void EnterState(EnemyFSM fsm)
    {
        _timer = 0;
        fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
        fsm.GFX.FlashQuestionMark();
    }

    public void Update(EnemyFSM fsm)
    {
        _timer += Time.deltaTime;

        if (_timer >= fsm.enemyData.timeFrozenAfterLOSBreak)
            fsm.SetState(fsm.states[EnemyFSM.StateType.PatrolState]);
    }

    public void FixedUpdate(EnemyFSM fsm) {}
    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}

    public void ExitState(EnemyFSM fsm) {}
}
