using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaLostLOSState : IEnemyState
{
    OkkaFSM _fsm;
    float _timer;

    public OkkaLostLOSState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }
    
    public void EnterState()
    {
        _timer = 0;
        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
        _fsm.GFX.FlashQuestionMark();
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _fsm.enemyData.timeFrozenAfterLOSBreak)
            _fsm.SetState(_fsm.states[EnemyFSM.StateType.PatrolState]);
    }

    public void FixedUpdate() {}
    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}
    public void ExitState() {}
}
