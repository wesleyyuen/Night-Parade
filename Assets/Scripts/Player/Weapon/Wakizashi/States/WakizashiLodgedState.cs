using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class WakizashiLodgedState : IWeaponState, IBindInput
{
    private WakizashiFSM _fsm;
    private Collider2D _collider;
    private Rigidbody2D _rb;
    private PlayerAnimations _playerAnimation;
    private int _layerMasks;
    private bool _isReturning, _stopUpdating;

    public WakizashiLodgedState(WakizashiFSM fsm)
    {
        _fsm = fsm;

        _rb = fsm.GetComponent<Rigidbody2D>();
        _collider = fsm.GetComponent<Collider2D>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
        _layerMasks = 1 << LayerMask.NameToLayer("Player");
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Throw.started += OnStartReturn;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Throw.started -= OnStartReturn;
    }

    public void EnterState()
    {
        _isReturning = false;
        _stopUpdating = false;
    }

    private void OnStartReturn(InputAction.CallbackContext context)
    {
        if (!_isReturning)
            _isReturning = true;
    }

    public void Update()
    {
        if (_isReturning && !_stopUpdating) {
            _stopUpdating = true;
            _fsm.SetState(_fsm.states[WeaponFSM.StateType.ReturnState]);
        }
    }

    public void FixedUpdate()
    {
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // Player retrieves wakizashi
        if (collider.gameObject.layer == LayerMask.NameToLayer("Player") && !_stopUpdating) {
            _playerAnimation.SetUnlodgedAnimation();

            // Reset wakizashi
            _fsm.transform.parent = _fsm.player.transform;
            _fsm.transform.localPosition = Vector3.zero;
            _fsm.transform.localRotation = Quaternion.identity;
            _collider.enabled = false;

            _fsm.SetState(_fsm.states[WeaponFSM.StateType.IdleState]);
        }
    }

    public void ExitState()
    {
    }
}
