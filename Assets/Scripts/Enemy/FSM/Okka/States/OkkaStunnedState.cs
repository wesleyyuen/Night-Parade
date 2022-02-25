using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OkkaStunnedState : IEnemyState
{
    private OkkaFSM _fsm;
    private float _stunnedDuration;
    private float _timer;
    
    public OkkaStunnedState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }

    public void EnterState()
    {
        if (float.TryParse(_fsm.stateParameter, out float duration)) {
            _stunnedDuration = duration;
        } else {
            _stunnedDuration = 0.5f;
        }
        _timer = 0;
    
        // _fsm.rb.velocity = Vector2.zero;
        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
        _fsm.LetRigidbodyMoveForSeconds(_stunnedDuration);
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _stunnedDuration) {
            
            // _fsm.SetState(_fsm.previousState);

            // if (Vector2.Distance(_fsm.player.attachedRigidbody.position, _fsm.rb.position) <= _fsm.enemyData.attackDistance && isInLOS) {
            //     _fsm.SetState(_fsm.states[OkkaStateType.Attack]);
            // }
            // else
            if (_fsm.IsInAggroRange() || _fsm.IsInLineOfSight()) {
                _fsm.SetState(_fsm.states[OkkaStateType.Aggro]);
            }
            else {
                _fsm.SetState(_fsm.states[OkkaStateType.Patrol]);
            }
        }
    }

    public void FixedUpdate() {}
    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision)
    {
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
    }

    public void ExitState()
    {
    }
}
