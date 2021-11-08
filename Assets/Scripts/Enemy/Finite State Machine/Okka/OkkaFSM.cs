using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaFSM : EnemyFSM
{
    public readonly OkkaPatrolState patrolState = new OkkaPatrolState();
    public readonly OkkaAggroState aggroState = new OkkaAggroState();
    // public readonly OkkaAttackState attackState = new OkkaAttackState();
    public readonly OkkaLostLOSState lostLOSState = new OkkaLostLOSState();
    public readonly OkkaStillState stillState = new OkkaStillState();
    public readonly OkkaStunnedState stunnedState = new OkkaStunnedState();
    public readonly OkkaDamagedState damagedState = new OkkaDamagedState();
    public readonly OkkaDeathState deathState = new OkkaDeathState();
    
    protected override void Awake()
    {
        base.Awake();
        enemyData = new OkkaData();

        states.Add(StateType.PatrolState, patrolState);
        states.Add(StateType.AggroState, aggroState);
        // states.Add(StateType.AttackState, _variant == OkkaVariant.Ignore ? null : attackState);
        states.Add(StateType.LostLOSState, lostLOSState);
        states.Add(StateType.StillState, stillState);
        states.Add(StateType.StunnedState, stunnedState);
        states.Add(StateType.DamagedState, damagedState);
        states.Add(StateType.DeathState, deathState);
    }

    protected override void Start()
    {
        base.Start();

        SetState(patrolState);
    }
}
