using UnityEngine;
using UnityEngine.InputSystem;

public sealed class WakizashiLodgedState : IWeaponState
{
    WakizashiFSM _fsm;
    PlayerAnimations _playerAnimation;
    int _layerMasks;
    public WakizashiLodgedState(WakizashiFSM fsm)
    {
        _fsm = fsm;

        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
        _layerMasks = 1 << LayerMask.NameToLayer("Player");
    }

    public void EnterState()
    {
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
        // Check Collision
        Collider2D hit = Physics2D.OverlapArea(_fsm.transform.position - new Vector3(0.5f, 0.5f),
                                               _fsm.transform.position + new Vector3(0.5f, 0.5f),
                                               _layerMasks);
                                               
        if (hit) {
            _fsm.SetState(_fsm.states[WeaponFSM.StateType.IdleState]);
        }
    }

    public void ExitState()
    {
        _playerAnimation.SetUnlodgedAnimation();
        _fsm.transform.parent = _fsm.player.transform;
        _fsm.transform.localPosition = Vector3.zero;
        _fsm.transform.localRotation = Quaternion.identity;
    }
}
