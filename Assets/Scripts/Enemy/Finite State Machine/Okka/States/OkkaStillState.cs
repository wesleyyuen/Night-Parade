using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OkkaStillState : IEnemyState
{
    private OkkaFSM _fsm;
    private float _timer;

    public OkkaStillState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }
    
    public void EnterState()
    {
        _timer = 0;
        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _fsm.enemyData.timeFrozenAfterDamagingPlayer) {
            if ((_fsm.IsInLineOfSight() || _fsm.IsInAggroRange())
                && _fsm.states[EnemyFSM.StateType.AggroState] != null)
                _fsm.SetState(_fsm.states[EnemyFSM.StateType.AggroState]);
            else
                _fsm.SetState(_fsm.states[EnemyFSM.StateType.PatrolState]);
        }
    }

    public void FixedUpdate() {}

    public void OnCollisionEnter2D(Collision2D collision) {}

    public void OnCollisionStay2D(Collision2D collision) {
        GameObject go = collision.gameObject;
        if (go.layer == LayerMask.NameToLayer("Player")) {
            _fsm.rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void OnCollisionExit2D(Collision2D collision) {
        GameObject go = collision.gameObject;
        if (go.layer == LayerMask.NameToLayer("Player")) {
            _fsm.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    public void ExitState()
    {
        _fsm.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
