using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkkaFSM : EnemyFSM
{
    OkkaPatrolState _patrolState;
    OkkaAggroState _aggroState;
    // OkkaAttackState _attackState;
    OkkaLostLOSState _lostLOSState;
    OkkaStillState _stillState;
    OkkaStunnedState _stunnedState;
    OkkaDamagedState _damagedState;
    OkkaDeathState _deathState;
    
    protected override void Awake()
    {
        _patrolState = new OkkaPatrolState(this);
        _aggroState = new OkkaAggroState(this);
        // _attackState = new OkkaAttackState(this);
        _lostLOSState = new OkkaLostLOSState(this);
        _stillState = new OkkaStillState(this);
        _stunnedState = new OkkaStunnedState(this);
        _damagedState = new OkkaDamagedState(this);
        _deathState = new OkkaDeathState(this);

        enemyData = new OkkaData();

        base.Awake();

        states.Add(StateType.PatrolState, _patrolState);
        states.Add(StateType.AggroState, _aggroState);
        // states.Add(StateType.AttackState, _variant == OkkaVariant.Ignore ? null : attackState);
        states.Add(StateType.LostLOSState, _lostLOSState);
        states.Add(StateType.StillState, _stillState);
        states.Add(StateType.StunnedState, _stunnedState);
        states.Add(StateType.DamagedState, _damagedState);
        states.Add(StateType.DeathState, _deathState);
    }

    protected override void Start()
    {
        base.Start();

        SetState(_patrolState);
    }
}
