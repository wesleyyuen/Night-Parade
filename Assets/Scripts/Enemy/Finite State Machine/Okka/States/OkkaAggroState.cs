using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class OkkaAggroState : IEnemyState
{
    OkkaFSM _fsm;
    float _delayTimer;

    public OkkaAggroState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }
    
    public void EnterState()
    {
        _delayTimer = 0;

        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", true);
        _fsm.GFX.SetAnimatorSpeed(_fsm.enemyData.aggroMovementSpeed / _fsm.enemyData.patrolSpeed * 0.75f);

        // Just notices player
        if (_fsm.previousState == _fsm.states[EnemyFSM.StateType.PatrolState])
            Timing.RunCoroutine(_NoticePlayer());
    }

    public void Update()
    {
        // check Line of Sight
        bool inLineOfSight = _fsm.IsInLineOfSight();
        // Always face player when aggro
        _fsm.GFX.FaceTowardsPlayer(0f);

        // Drop aggro after a certain delay period without LOS
        if (!inLineOfSight) {
            _delayTimer += Time.deltaTime;

            if (_delayTimer >= _fsm.enemyData.lineOfSightBreakDelay)
                _fsm.SetState(_fsm.states[EnemyFSM.StateType.LostLOSState]);

        } else {
            _delayTimer = 0;
        }

        // if (!fsm.GFX.IsTurning()) {
        //     if (Vector2.Distance(fsm.player.attachedRigidbody.position, fsm.rb.position) <= fsm.enemyData.attackDistance && fsm.IsInLineOfSight())
        //         fsm.SetState(fsm.states[EnemyFSM.StateType.AttackState]);
        // }
    }

    public void FixedUpdate()
    {
        if (!_fsm.GFX.IsTurning())
            MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector2 target = new Vector2 (_fsm.player.bounds.center.x, _fsm.rb.position.y);
        
        Vector2 direction = (target - (Vector2) _fsm.transform.position).normalized;
        _fsm.rb.velocity = new Vector2(direction.x * _fsm.enemyData.aggroMovementSpeed , _fsm.rb.velocity.y);
    }

    IEnumerator<float> _NoticePlayer()
    {
        _fsm.GFX.FaceTowardsPlayer(0f);

        _fsm.StunForSeconds(0.4f);
        yield return Timing.WaitForSeconds(0.2f);
        _fsm.GFX.FlashExclaimationMark();

        // jump slightly
        _fsm.rb.velocity = Vector2.zero;
        _fsm.rb.velocity += Vector2.up * 10f;
    }

    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}

    public void ExitState()
    {
        _fsm.GFX.SetAnimatorSpeed(1f);
    }
}
