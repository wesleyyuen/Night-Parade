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
        _attackAngle = new Vector2(fsm.player.attachedRigidbody.position.x >= fsm.rb.position.x ? 1f : -1f, 0f);

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
        float time = _fsm.enemyData.attackChargeTime * 0.4f,
              timer = 0f,
              speed = _fsm.enemyData.aggroMovementSpeed;

        bool facingRight = _fsm.GFX.GetEnemyScale().x > 0;
        _fsm.rb.velocity = Vector2.zero;
        _fsm.rb.angularVelocity = 0f;

        // Wind up
        while (timer < time) {
            timer += Time.deltaTime;
            _fsm.rb.velocity = new Vector2((facingRight ? -1f : 1f) * speed, 0f);
            yield return null;
        }

        // Stop Winding
        _fsm.rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(_fsm.enemyData.attackChargeTime * 0.6f);

        // Lunge
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
