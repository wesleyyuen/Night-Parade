using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class BowNockState : IWeaponState
{
    private BowFSM _fsm;
    private PlayerAnimations _playerAnimation;
    private const float NOCK_DURATION = 1f;
    private float _timer;

    public BowNockState(BowFSM fsm)
    {
        _fsm = fsm;

        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void EnterState()
    {
        _timer = 0f;
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= NOCK_DURATION) {
            _fsm.SetState(_fsm.states[BowStateType.Idle]);
        }
    }

    public void FixedUpdate()
    {
    }

    public void ExitState()
    {
        _timer = 0f;
    }
}
