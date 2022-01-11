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

        // Align Wakizashi perpendicular to lodged surface
        // Lodged In Ground
        if (_fsm.transform.parent.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            _fsm.transform.localRotation = Quaternion.Euler(0f, 0f, _fsm.isThrownRight ? -90f : 90f);
        }
        // Lodged In Wall
        else if (_fsm.transform.parent.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            _fsm.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        // Lodged In Enemy
        else if (_fsm.transform.parent.gameObject.layer == LayerMask.NameToLayer("Enemies")) {
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
        // Instantly return
        if (Vector2.Distance(_fsm.transform.position, _fsm.player.position) >= 25f && !_stopUpdating) {
            _stopUpdating = true;
            ReturnInstantly();
        }

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
            _stopUpdating = true;
            RetrieveWakizashi();
        }
    }

    private void RetrieveWakizashi()
    {
        _playerAnimation.SetUnlodgedAnimation();
        ResetWakizashi();
        _fsm.SetState(_fsm.states[WakizashiStateType.Idle]);
    }

    private void ReturnInstantly()
    {
        // TODO: Add VFX Effects
        RetrieveWakizashi();
    }

    private void ResetWakizashi()
    {
        _fsm.transform.parent = _fsm.player.transform;
        _fsm.transform.localPosition = Vector3.zero;
        _fsm.transform.localRotation = Quaternion.identity;
        _collider.enabled = false;
    }

    public void ExitState()
    {
    }
}
