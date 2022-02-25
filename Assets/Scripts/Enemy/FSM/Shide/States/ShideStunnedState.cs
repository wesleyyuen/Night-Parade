using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ShideStunnedState : IEnemyState
{
    private ShideFSM _fsm;
    private float _stunnedDuration;
    private float _timer;
    private bool _stopUpdating;
    
    public ShideStunnedState(ShideFSM fsm)
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
        _stopUpdating = false;
    
        _fsm.LetRigidbodyMoveForSeconds(_stunnedDuration);
        _fsm.rb.velocity = Vector2.zero;
        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);
    }

    public void Update()
    {
        if (_stopUpdating) return;

        if (_timer >= _stunnedDuration) {

            // _fsm.SetState(_fsm.previousState);

            // if (Vector2.Distance(_fsm.player.attachedRigidbody.position, _fsm.rb.position) <= _fsm.enemyData.attackDistance && isInLOS) {
            //     _fsm.SetState(_fsm.states[ShideStateType.Attack]);
            // }
            // else
            if (_fsm.IsInAggroRange() || _fsm.IsInLineOfSight()) {
                _stopUpdating = true;
                _fsm.SetState(_fsm.states[ShideStateType.Aggro]);
            }
            else {
                _stopUpdating = true;
                _fsm.SetState(_fsm.states[ShideStateType.Patrol]);
            }
        }

        _timer += Time.deltaTime;
    }

    public void FixedUpdate()
    {
        _fsm.rb.velocity = Vector2.zero;
    }
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
