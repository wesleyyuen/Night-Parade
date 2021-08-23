using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaDeathState : EnemyState
{
    float _timer;
    public override void EnterState(EnemyFSM fsm)
    {
        _timer = 0;

        // Apply Knock back
        fsm.rb.drag = fsm.enemyData.knockBackOnAttackedForce * 1.5f * 0.1f;
        bool playerOnLeft = fsm.rb.position.x > fsm.player.transform.position.x;
        fsm.rb.AddForce(fsm.enemyData.knockBackOnAttackedForce * 1.5f * (playerOnLeft ? Vector2.right : Vector2.left), ForceMode2D.Impulse);

        Physics2D.IgnoreCollision(fsm.player, fsm.col);
        fsm.GFX.PlayDeathEffect(fsm.enemyData.dieTime);
    }

    public override void Update(EnemyFSM fsm)
    {
        _timer += Time.deltaTime;

        if (_timer >= fsm.enemyData.dieTime) {
            fsm.Die();
        }
    }

    public override void FixedUpdate(EnemyFSM fsm) {}
    public override void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public override void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public override void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}

    public override void ExitState(EnemyFSM fsm) {}
}
