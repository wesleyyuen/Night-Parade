using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaDeathState : IEnemyState
{
    float _timer;
    public void EnterState(EnemyFSM fsm)
    {
        _timer = 0;
        Physics2D.IgnoreCollision(fsm.player, fsm.col);

        // Stop all forces previously applied to it
        fsm.rb.velocity = Vector2.zero;
        fsm.rb.angularVelocity = 0f;

        // Apply Knock back
        float knockBackForce = fsm.enemyData.knockBackOnTakingDamageForce * 1.5f;
        bool playerOnLeft = fsm.rb.position.x > fsm.player.transform.position.x;
        fsm.ApplyForce(playerOnLeft ? Vector2.right : Vector2.left, fsm.enemyData.knockBackOnTakingDamageForce * 1.5f);

        fsm.GFX.PlayDeathEffect(fsm.enemyData.dieTime);
    }

    public void Update(EnemyFSM fsm)
    {
        _timer += Time.deltaTime;

        if (_timer >= fsm.enemyData.dieTime)
            fsm.Die();
    }

    public void FixedUpdate(EnemyFSM fsm) {}
    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}
    public void ExitState(EnemyFSM fsm) {}
}
