using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public sealed class OkkaAttackState : IEnemyState
{
    private OkkaFSM _fsm;
    private float _timer;
    private bool _hasAttacked;
    private Vector2 _attackAngle;

    public OkkaAttackState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }
    
    public void EnterState()
    {
        _hasAttacked = false;
        _attackAngle = new Vector2(_fsm.player.attachedRigidbody.position.x >= _fsm.rb.position.x ? 1f : -1f, 0f);

        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);

        Timing.RunCoroutine(_Attack());
    }

    public void Update()
    {
        if (_hasAttacked) {
            _timer += Time.deltaTime;

            if (_timer >= _fsm.enemyData.attackTime)
                _fsm.SetState(_fsm.states[EnemyFSM.StateType.AggroState]);
        }
    }

    private IEnumerator<float> _Attack()
    {
        float time = _fsm.enemyData.attackChargeTime * 0.4f,
              timer = 0f,
              speed = _fsm.enemyData.aggroMovementSpeed;

        bool facingRight = _fsm.GFX.GetEnemyScale().x > 0;
        _fsm.rb.velocity = Vector2.zero;
        _fsm.rb.angularVelocity = 0f;

        // Wind up
        while (timer < time) {
            timer += Timing.DeltaTime;
            _fsm.rb.velocity = new Vector2((facingRight ? -0.5f : 0.5f) * speed, 0f);
            yield return Timing.WaitForOneFrame;
        }

        // Stop Winding
        _fsm.rb.velocity = Vector2.zero;
        yield return Timing.WaitForSeconds(_fsm.enemyData.attackChargeTime * 0.6f);

        // Lunge
        _fsm.ApplyForce(_attackAngle, _fsm.enemyData.attackForce, _fsm.enemyData.attackTime);
        _hasAttacked = true;
    }

    public void FixedUpdate() {}
    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}
    public void ExitState() {}
}
