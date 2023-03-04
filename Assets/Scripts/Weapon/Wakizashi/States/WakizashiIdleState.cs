using UnityEngine;

// Include both idle + moving states
public sealed class WakizashiIdleState : IWeaponState, IBindInput
{
    private InputManager _inputManager;
    private WakizashiFSM _fsm;
    private Rigidbody2D _rb;
    private bool _stopUpdating;

    public WakizashiIdleState(WakizashiFSM fsm, InputManager inputManager)
    {
        _fsm = fsm;
        _inputManager = inputManager;

        _rb = fsm.GetComponent<Rigidbody2D>();
    }

    public void BindInput()
    {
        _inputManager.Event_GameplayInput_Attack += OnStartAttack;
        _inputManager.Event_GameplayInput_ThrowTap += OnStartThrow;
        // _inputManager.Event_GameplayInput_ThrowHold += OnStartAim;
    }

    public void UnbindInput()
    {
        _inputManager.Event_GameplayInput_Attack -= OnStartAttack;
        _inputManager.Event_GameplayInput_ThrowTap -= OnStartThrow;
        // _inputManager.Event_GameplayInput_ThrowHold -= OnStartAim;
    }

    public void EnterState()
    {
        _stopUpdating = false;

        // Reset forces and transform
        _rb.velocity = Vector2.zero;
        _fsm.transform.localPosition = Vector3.zero;
        _fsm.transform.localEulerAngles = Vector3.zero;
    }

    private void OnStartAttack()
    {
        if (!_stopUpdating && _fsm.IsCurrentState(WakizashiStateType.Idle) && _fsm.canAttack && _fsm.attackCooldownTimer <= 0) {
            _stopUpdating = true;
            _fsm.SetState(_fsm.states[WakizashiStateType.Attack]);
        }
    }

    private void OnStartThrow()
    {
        if (!_stopUpdating && _fsm.IsCurrentState(WakizashiStateType.Idle) && _fsm.throwCooldownTimer <= 0) {
            _stopUpdating = true;
            _fsm.SetState(_fsm.states[WakizashiStateType.Throw]);
        }
    }

    // private void OnStartAim()
    // {
    //     if (!_stopUpdating && _fsm.IsCurrentState(WakizashiStateType.Idle) && _fsm.throwCooldownTimer <= 0) {
    //         _stopUpdating = true;
    //         _fsm.SetState(_fsm.states[WakizashiStateType.Aim]);
    //     }
    // }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }

    public void ExitState()
    {
    }
}
