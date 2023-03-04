public class OkkaFSM : EnemyFSM
{
    private OkkaPatrolState _patrolState;
    private OkkaAggroState _aggroState;
    // private OkkaAttackState _attackState;
    private OkkaStunnedState _stunnedState;
    private OkkaDeathState _deathState;
    
    protected override void Awake()
    {
        _patrolState = new OkkaPatrolState(this);
        _aggroState = new OkkaAggroState(this);
        // _attackState = new OkkaAttackState(this);
        _stunnedState = new OkkaStunnedState(this);
        _deathState = new OkkaDeathState(this);

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
}

public sealed class OkkaStateType : RegularEnemyStateType
{
    private OkkaStateType(string value) : base(value) { }
}
