using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaDamagedState : EnemyState
{
    float _timer;
    public override void EnterState(EnemyFSM fsm)
    {
        _timer = 0;

        fsm.GFX.SetAnimatorBoolean("IsPatrolling", false);

        // Play Damaged Effect
        fsm.GFX.PlayDamagedEffect();

        // Apply Knock back
        fsm.rb.drag = fsm.enemyData.knockBackOnAttackedForce * 0.1f;
        bool playerOnLeft = fsm.rb.position.x > fsm.player.transform.position.x;
        fsm.rb.AddForce(fsm.enemyData.knockBackOnAttackedForce * (playerOnLeft ? Vector2.right : Vector2.left), ForceMode2D.Impulse);
    }

    public override void Update(EnemyFSM fsm)
    {
        _timer += Time.deltaTime;

        if (_timer >= fsm.enemyData.timeFrozenAfterTakingDamage) {
            fsm.SetState(fsm.states[EnemyFSM.StateType.AggroState]);
        }
    }

    public override void FixedUpdate(EnemyFSM fsm) {}
    public override void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public override void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public override void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}

    public override void ExitState(EnemyFSM fsm)
    {
        fsm.rb.drag = 0f;
    }
}
