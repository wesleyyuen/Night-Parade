using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public sealed class ShideStateType : RegularEnemyStateType
{
    private ShideStateType(string value) : base(value) { }
}

public class ShideFSM : EnemyFSM
{
    private ShidePatrolState _patrolState;
    private ShideAggroState _aggroState;
    // private ShideAttackState _attackState;
    private ShideStunnedState _stunnedState;
    private ShideDeathState _deathState;
    private Tweener _applyForceTweener;
    
    protected override void Awake()
    {
        _patrolState = new ShidePatrolState(this);
        _aggroState = new ShideAggroState(this);
        // _attackState = new ShideAttackState(this);
        _stunnedState = new ShideStunnedState(this);
        _deathState = new ShideDeathState(this);

        base.Awake();

        states.Add(RegularEnemyStateType.Patrol, _patrolState);
        states.Add(RegularEnemyStateType.Aggro, _aggroState);
        // states.Add(RegularEnemyStateType.Attack, _attackState);
        states.Add(RegularEnemyStateType.Stunned, _stunnedState);
        states.Add(RegularEnemyStateType.Death, _deathState);

        SetState(_patrolState);
    }

    public override bool IsAttacking()
    {
        // return IsCurrentState(RegularEnemyStateType.Aggro);
        return false;
    }

    public override void ApplyForce(Vector2 dir, float force, float time = 0)
    {
        _applyForceTweener?.Kill();
        // Debug.Log(dir * force);
        LetRigidbodyMoveForSeconds(time);
        _applyForceTweener = rb.DOMove(rb.position + dir * force, time).SetEase(DG.Tweening.Ease.OutCirc);
    }
}
