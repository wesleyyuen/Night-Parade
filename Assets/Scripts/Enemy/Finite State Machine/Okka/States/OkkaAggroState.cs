using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaAggroState : IEnemyState
{
    float _delayTimer;
    OkkaFSM _fsm;
    public void EnterState(EnemyFSM fsm)
    {
        _delayTimer = 0;
        _fsm = (OkkaFSM) fsm;

        fsm.GFX.SetAnimatorBoolean("IsPatrolling", true);
        fsm.GFX.SetAnimatorSpeed(fsm.enemyData.aggroMovementSpeed / fsm.enemyData.patrolSpeed * 0.75f);

        // Just notices player
        if (fsm.previousState == fsm.states[EnemyFSM.StateType.PatrolState])
            Utility.StaticCoroutine.Start(NoticePlayer());
    }

    public void Update(EnemyFSM fsm)
    {
        // check Line of Sight
        bool inLineOfSight = fsm.IsInLineOfSight();
        // Always face player when aggro
        fsm.GFX.FaceTowardsPlayer(0f);

        // Drop aggro after a certain delay period without LOS
        if (!inLineOfSight) {
            _delayTimer += Time.deltaTime;

            if (_delayTimer >= fsm.enemyData.lineOfSightBreakDelay)
                fsm.SetState(fsm.states[EnemyFSM.StateType.LostLOSState]);

        } else {
            _delayTimer = 0;
        }

        // if (!fsm.GFX.IsTurning()) {
        //     if (Vector2.Distance(fsm.player.attachedRigidbody.position, fsm.rb.position) <= fsm.enemyData.attackDistance && fsm.IsInLineOfSight())
        //         fsm.SetState(fsm.states[EnemyFSM.StateType.AttackState]);
        // }
    }

    public void FixedUpdate(EnemyFSM fsm)
    {
        if (!fsm.GFX.IsTurning())
            MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        Vector2 target = new Vector2 (_fsm.player.bounds.center.x, _fsm.rb.position.y);
        
        Vector2 direction = (target - (Vector2) _fsm.transform.position).normalized;
        _fsm.rb.velocity = new Vector2(direction.x * _fsm.enemyData.aggroMovementSpeed , _fsm.rb.velocity.y);
    }

    IEnumerator NoticePlayer()
    {
        _fsm.GFX.FaceTowardsPlayer(0f);

        _fsm.StunForSeconds(0.4f);
        yield return new WaitForSeconds(0.2f);
        _fsm.GFX.FlashExclaimationMark();

        // jump slightly
        _fsm.rb.velocity = Vector2.zero;
        _fsm.rb.velocity += Vector2.up * 10f;
    }

    public void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision) {}
    public void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision) {}
    
    public void ExitState(EnemyFSM fsm)
    {
        fsm.GFX.SetAnimatorSpeed(1f);
    }
}
