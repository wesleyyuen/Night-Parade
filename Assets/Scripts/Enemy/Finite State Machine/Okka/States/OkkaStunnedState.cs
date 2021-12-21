using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaStunnedState : IEnemyState
{
    public float stunnedDuration;
    OkkaFSM _fsm;
    float _timer;
    
    public OkkaStunnedState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }

    public void EnterState()
    {
        _timer = 0;
        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
        _fsm.LetRigidbodyMoveForSeconds(stunnedDuration);
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= stunnedDuration) {
            bool isInLOS = _fsm.IsInLineOfSight();

            /*if (Vector2.Distance(_fsm.player.attachedRigidbody.position, _fsm.rb.position) <= _fsm.enemyData.attackDistance
                && isInLOS
                && _fsm.states[EnemyFSM.StateType.AttackState] != null)
                _fsm.SetState(_fsm.states[EnemyFSM.StateType.AttackState]);
            else */
            if ((isInLOS || _fsm.IsInAggroRange()) && _fsm.states[EnemyFSM.StateType.AggroState] != null)
                _fsm.SetState(_fsm.states[EnemyFSM.StateType.AggroState]);
            else
                _fsm.SetState(_fsm.states[EnemyFSM.StateType.PatrolState]);
        }
    }

    public void FixedUpdate() {}
    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}
    public void ExitState() {}
}
