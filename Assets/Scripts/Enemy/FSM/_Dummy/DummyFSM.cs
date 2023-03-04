using UnityEngine;

public class DummyFSM : EnemyFSM
{
    private DummyStunnedState _stunnedState;
    private DummyPatrolState _patrolState;

    protected override void Awake()
    {
        _stunnedState = new DummyStunnedState(this);
        _patrolState = new DummyPatrolState(this);

        base.Awake();

        states.Add(RegularEnemyStateType.Stunned, _stunnedState);
        states.Add(RegularEnemyStateType.Patrol, _patrolState);

        SetState(_patrolState);
    }

    public override bool IsAttacking()
    {
        return false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (player == null) return;
        
        _currentState?.OnCollisionEnter2D(collision);
    }

    protected override void OnCollisionStay2D(Collision2D collision)
    {
        if (player == null) return;

        _currentState?.OnCollisionStay2D(collision);
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        if (player == null) return;

        _currentState?.OnCollisionExit2D(collision);
    }
}

public sealed class DummyStateType : RegularEnemyStateType
{
    private DummyStateType(string value) : base(value) { }
}