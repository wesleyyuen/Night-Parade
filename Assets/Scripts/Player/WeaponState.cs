public interface IState
{
    public void EnterState(WeaponFSM fsm);
    public void Update(WeaponFSM fsm);
    public void FixedUpdate(WeaponFSM fsm);
    public void ExitState(WeaponFSM fsm);
}
