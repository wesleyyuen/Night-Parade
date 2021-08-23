public abstract class WeaponState
{
    public abstract void EnterState(WeaponFSM fsm);
    public abstract void Update(WeaponFSM fsm);
    public abstract void FixedUpdate(WeaponFSM fsm);
    public abstract void ExitState(WeaponFSM fsm);
}
