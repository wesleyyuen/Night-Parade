using UnityEngine;

public abstract class EnemyState
{
    public abstract void EnterState(EnemyFSM fsm);
    public abstract void Update(EnemyFSM fsm);
    public abstract void FixedUpdate(EnemyFSM fsm);
    public abstract void OnCollisionEnter2D(EnemyFSM fsm, Collision2D collision);
    public abstract void OnCollisionStay2D(EnemyFSM fsm, Collision2D collision);
    public abstract void OnCollisionExit2D(EnemyFSM fsm, Collision2D collision);
    public abstract void ExitState(EnemyFSM fsm);
}