using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaDeathState : IEnemyState
{
    OkkaFSM _fsm;
    float _timer;

    public OkkaDeathState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }
    
    public void EnterState()
    {
        _timer = 0;
        Physics2D.IgnoreCollision(_fsm.player, _fsm.col);

        // Stop all forces previously applied to it
        _fsm.rb.velocity = Vector2.zero;
        _fsm.rb.angularVelocity = 0f;

        // Apply Knock back
        float knockBackForce = _fsm.enemyData.knockBackOnTakingDamageForce * 1.5f;
        bool playerOnLeft = _fsm.rb.position.x > _fsm.player.transform.position.x;
        _fsm.ApplyForce(playerOnLeft ? Vector2.right : Vector2.left, _fsm.enemyData.knockBackOnTakingDamageForce * 1.5f);

        _fsm.GFX.PlayDeathEffect(_fsm.enemyData.dieTime);
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _fsm.enemyData.dieTime)
            _fsm.Die();
    }

    public void FixedUpdate() {}
    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}
    public void ExitState() {}
}
