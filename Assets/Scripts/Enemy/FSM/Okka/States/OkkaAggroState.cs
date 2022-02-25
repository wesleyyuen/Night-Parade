using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public sealed class OkkaAggroState : IEnemyState
{
    private OkkaFSM _fsm;
    private float _delayTimer;
    private bool _stopUpdating;

    public OkkaAggroState(OkkaFSM fsm)
    {
        _fsm = fsm;
    }
    
    public void EnterState()
    {
        _delayTimer = 0;
        _stopUpdating = false;

        _fsm.GFX.FaceTowardsPlayer(0f);
        _fsm.GFX.SetAnimatorBoolean("IsPatrolling", true);
        _fsm.GFX.SetAnimatorSpeed(_fsm.enemyData.aggroMovementSpeed / _fsm.enemyData.patrolSpeed * 0.75f);

        // Just notices player
        if (_fsm.IsPreviousState(OkkaStateType.Patrol)) {
            Timing.RunCoroutine(_NoticePlayer());
        }
    }

    public void Update()
    {
        // check Line of Sight
        bool inLineOfSight = _fsm.IsInLineOfSight();

        // Drop aggro after a certain delay period without LOS
        if (!inLineOfSight) {
            _delayTimer += Time.deltaTime;

            if (!_stopUpdating && _delayTimer >= _fsm.enemyData.lineOfSightBreakDelay) {
                _stopUpdating = true;
                _fsm.StunForSeconds(_fsm.enemyData.timeStunnedAfterLOSBreak);
            }
        } else {
            _delayTimer = 0;
        }

        // if (!_stopUpdating && !_fsm.GFX.IsTurning()) {
        //     if (Vector2.Distance(_fsm.player.attachedRigidbody.position, _fsm.rb.position) <= _fsm.enemyData.attackDistance && inLineOfSight) {
        //         _stopUpdating = true;
        //         _fsm.SetState(_fsm.states[OkkaStateType.Attack]);
        //     }
        // }
    }

    public void FixedUpdate()
    {
        if (!_stopUpdating && !_fsm.GFX.IsTurning())
            MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        Vector2 target = new Vector2 (_fsm.player.bounds.center.x, _fsm.rb.position.y);
        
        Vector2 direction = (target - (Vector2) _fsm.transform.position).normalized;
        _fsm.rb.velocity = new Vector2(direction.x * _fsm.enemyData.aggroMovementSpeed, _fsm.rb.velocity.y);
    }

    private IEnumerator<float> _NoticePlayer()
    {
        _stopUpdating = true;
        _fsm.GFX.FaceTowardsPlayer(0f);

        // _fsm.StunForSeconds(0.3f);
        yield return Timing.WaitForSeconds(0.3f);
        // _fsm.GFX.FlashExclaimationMark();

        // jump slightly
        // _fsm.rb.velocity = Vector2.up * 10f;
        // yield return Timing.WaitForSeconds(0.3f);
        // _fsm.rb.velocity = Vector2.zero;
        _stopUpdating = false;
    }

    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}

    public void ExitState()
    {
        _fsm.GFX.SetAnimatorSpeed(1f);
    }
}
