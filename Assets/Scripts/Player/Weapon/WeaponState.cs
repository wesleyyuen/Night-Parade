public interface IWeaponState
{
    void Awake(WeaponFSM fsm);
    void EnterState(WeaponFSM fsm);
    void Update(WeaponFSM fsm);
    void FixedUpdate(WeaponFSM fsm);
    void ExitState(WeaponFSM fsm);
}
