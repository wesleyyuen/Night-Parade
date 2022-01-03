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
        _fsm.InputActions.Player.Throw.performed += OnStartReturn;
        _fsm.InputActions.Player.Throw.canceled += OnStartReturn;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Throw.performed -= OnStartReturn;
        _fsm.InputActions.Player.Throw.canceled -= OnStartReturn;
    }

    public void EnterState()
    {
        _isReturning = false;
        _stopUpdating = false;

        // Lodged In Ground
        if (_fsm.transform.parent.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            _fsm.transform.localRotation = Quaternion.Euler(0f, 0f, _fsm.isThrownRight ? -90f : 90f);
        }
        // Lodged In Wall
        else if (_fsm.transform.parent.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            _fsm.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
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
            _fsm.SetState(_fsm.states[WakizashiStateType.Return]);
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

            _fsm.SetState(_fsm.states[WakizashiStateType.Idle]);
        }
    }

    public void ExitState()
    {
    }
}
