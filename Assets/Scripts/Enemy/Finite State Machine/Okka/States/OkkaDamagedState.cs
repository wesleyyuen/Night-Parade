using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaDamagedState : IEnemyState
{
    float _timer;
    public void EnterState(EnemyFSM fsm)
    {
        _timer = 0;

        fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);

        // Play Damaged Effect
        fsm.GFX.PlayDamagedEffect();

        // Apply Knock back
        bool playerOnLeft = fsm.rb.position.x > fsm.player.transform.position.x;
        fsm.ApplyForce(playerOnLeft ? Vector2.right : Vector2.left, fsm.enemyData.knockBackOnTakingDamageForce, fsm.enemyData.timeFrozenAfterTakingDamage);
    }

    public void Update(EnemyFSM fsm)
    {
        _timer += Time.deltaTime;

        if (_timer >= fsm.enemyData.timeFrozenAfterTakingDamage) {
            if (fsm.states[EnemyFSM.StateType.AggroState] != null)
                fsm.SetState(fsm.states[EnemyFSM.StateType.AggroState]);
            else
                fsm.SetState(fsm.states[EnemyFSM.StateType.PatrolState]);
        }
    }

    public void FixedUpdate(EnemyFSM fsm) {}
    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}
    public void ExitState(EnemyFSM fsm) {}
}
