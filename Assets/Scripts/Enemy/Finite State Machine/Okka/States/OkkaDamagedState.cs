using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaDamagedState : IEnemyState
{
    OkkaFSM _fsm;
    float _timer;

    public OkkaDamagedState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }
    
    public void EnterState()
    {
        _timer = 0;

        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);

        // Play Damaged Effect
        _fsm.GFX.PlayDamagedEffect();

        // Apply Knock back
        bool playerOnLeft = _fsm.rb.position.x > _fsm.player.transform.position.x;
        _fsm.ApplyForce(playerOnLeft ? Vector2.right : Vector2.left, _fsm.enemyData.knockBackOnTakingDamageForce, _fsm.enemyData.timeFrozenAfterTakingDamage);
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _fsm.enemyData.timeFrozenAfterTakingDamage) {
            if (_fsm.states[EnemyFSM.StateType.AggroState] != null)
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
