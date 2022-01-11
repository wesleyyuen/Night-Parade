using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class WakizashiFallState : IWeaponState, IBindInput
{
    private WakizashiFSM _fsm;
    private Collider2D _collider;
    private Rigidbody2D _rb;
    private PlayerAnimations _playerAnimation;
    private bool _isReturning, _stopUpdating;

    public WakizashiFallState(WakizashiFSM fsm)
    {
        _fsm = fsm;

        _rb = fsm.GetComponent<Rigidbody2D>();
        _collider = fsm.GetComponent<Collider2D>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
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

        // play animation
        // TODO: should be called from player animation
        _playerAnimation.SetThrowAnimation();
        _rb.isKinematic = false;
        _collider.enabled = true;

        // Detach from enemy
        _fsm.transform.parent = null;
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
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collider.gameObject.layer == LayerMask.NameToLayer("Wall") ||
            collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            HandleHit(collider.gameObject);
    }

    private void HandleHit(GameObject hit)
    {
        // Damage Enemy
        if (hit.gameObject.TryGetComponent<EnemyFSM>(out EnemyFSM enemy)) {
            WakizashiData data = (WakizashiData)_fsm.weaponData;
            enemy.TakeDamage(data.throwDamage, _rb.velocity);
        }

        // Lodge it
        if (!_isReturning && !_stopUpdating) {
            _fsm.transform.parent = hit.gameObject.transform;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;

            _fsm.SetState(_fsm.states[WakizashiStateType.Lodged]);
        }
    }

    public void ExitState()
    {
    }
}
