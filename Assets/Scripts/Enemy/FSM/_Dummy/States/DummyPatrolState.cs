using UnityEngine;

public sealed class DummyPatrolState : IEnemyState
{
    public DummyPatrolState(DummyFSM fsm)
    {
    }

    public void EnterState()
    {
    }

    public void OnCollisionEnter2D(Collision2D collision) {}
    public void OnCollisionStay2D(Collision2D collision) {}
    public void OnCollisionExit2D(Collision2D collision) {}
    public void ExitState() {}

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }
}
