using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaLostLOSState : EnemyState
{
    float _timer;
    public override void EnterState(EnemyFSM fsm)
    {
        _timer = 0;
        fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
        fsm.GFX.FlashQuestionMark();
    }

    public override void Update(EnemyFSM fsm)
    {
        _timer += Time.deltaTime;

        if (_timer >= fsm.enemyData.timeFrozenAfterLOSBreak)
            fsm.SetState(fsm.states[EnemyFSM.StateType.PatrolState]);
    }

    public override void FixedUpdate(EnemyFSM fsm) {}
    public override void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public override void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public override void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}

    public override void ExitState(EnemyFSM fsm) {}
}
