using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaAttackState : IEnemyState
{
    OkkaFSM _fsm;
    float _timer;
    bool _hasAttacked;
    Vector2 _attackAngle;
    public void EnterState(EnemyFSM fsm)
    {
        _fsm = (OkkaFSM) fsm;
        _hasAttacked = false;
        _attackAngle = new Vector2(fsm.player.attachedRigidbody.position.x >= fsm.rb.position.x ? 1f : -1f, 1f);

        fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);

        Utility.StaticCoroutine.Start(Attack());
    }

    public void Update(EnemyFSM fsm)
    {
        if (_hasAttacked) {
            _timer += Time.deltaTime;

            if (_timer >= fsm.enemyData.attackTime)
                fsm.SetState(fsm.states[EnemyFSM.StateType.AggroState]);
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(_fsm.enemyData.attackChargeTime);
        _fsm.ApplyForce(_attackAngle, _fsm.enemyData.attackForce, _fsm.enemyData.attackTime);
        _hasAttacked = true;
    }

    public void FixedUpdate(EnemyFSM fsm) {}
    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}
    
    public void ExitState(EnemyFSM fsm)
    {
    }
}
