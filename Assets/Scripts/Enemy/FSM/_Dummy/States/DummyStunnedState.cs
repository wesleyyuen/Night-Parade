using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DummyStunnedState : IEnemyState
{
    private DummyFSM _fsm;
    private float _stunnedDuration;
    private float _timer;
    
    public DummyStunnedState(DummyFSM fsm)
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
    
        _fsm.LetRigidbodyMoveForSeconds(_stunnedDuration);
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _stunnedDuration) {
            _fsm.SetState(_fsm.states[DummyStateType.Patrol]);
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
