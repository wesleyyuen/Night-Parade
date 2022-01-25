using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MEC;

public sealed class WakizashiThrowState : IWeaponState, IBindInput
{
    private WakizashiFSM _fsm;
    private WakizashiData _data;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerAnimations _playerAnimation;
    private bool _isReturning, _stopUpdating;

    public WakizashiThrowState(WakizashiFSM fsm)
    {
        _fsm = fsm;
        _data = (WakizashiData)fsm.weaponData;

        _rb = fsm.GetComponent<Rigidbody2D>();
        _collider = fsm.GetComponent<Collider2D>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Throw_Tap.started += OnStartReturn;
        _fsm.InputActions.Player.Throw_SlowTap.performed += OnStartReturn;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Throw_Tap.started += OnStartReturn;
        _fsm.InputActions.Player.Throw_SlowTap.performed -= OnStartReturn;
    }
 
    public void EnterState()
    {
        _isReturning = false;
        _stopUpdating = false;
        bool wasAiming = _fsm.previousState == _fsm.states[WakizashiStateType.Aim];

        if (_fsm.throwDirection == Vector2.zero) {
            _fsm.throwDirection = _playerAnimation.IsFacingRight() ? Vector2.right : Vector2.left;
            if (wasAiming) _fsm.throwDirection += new Vector2(0f, 0.15f);
            _fsm.throwDirection.Normalize();
        }
        
        _fsm.throwCooldownTimer = _data.throwCooldown;

        // play animation
        Utility.FreezePlayer(true, _data.throwMinDuration);
        _playerAnimation.SetThrowAnimation();

        _collider.enabled = true;

        // Detach from player
        _fsm.transform.localPosition = new Vector2(0f, 1.5f);
        _fsm.transform.parent = null;

        // Actually Throw
        Timing.RunCoroutine(_ActuallyThrow(!wasAiming).CancelWith(_fsm.gameObject));
    }

    private void OnStartReturn(InputAction.CallbackContext context)
    {
        if (_fsm.IsCurrentState(WakizashiStateType.Throw) && !_isReturning)
            _isReturning = true;
    }

    private IEnumerator<float> _ActuallyThrow(bool shouldSelfReturn)
    {
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;

        // Straight Trajectory
        _rb.velocity = _fsm.throwDirection * _data.throwVelocity;

        yield return Timing.WaitForSeconds(_data.throwMinDuration);

        if (shouldSelfReturn) {
            _isReturning = true;
        } else {
            // Apply Gravity
            _rb.isKinematic = false;
        }
    }

    public void Update()
    {
        if (_stopUpdating) return;

        if (_isReturning) {
            _stopUpdating = true;
            _fsm.SetState(_fsm.states[WakizashiStateType.Return]);
        }

        // Rotate
        _rb.transform.localEulerAngles += (_fsm.throwDirection.x > 0f ? Vector3.back : Vector3.forward) * 2700f * Time.deltaTime; 
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
        // Lodge it in
        if (!_isReturning && !_stopUpdating) {
            _fsm.transform.parent = hit.gameObject.transform;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;

            _fsm.SetState(_fsm.states[WakizashiStateType.Lodged]);
        }

        // Damage Enemy
        if (hit.gameObject.TryGetComponent<EnemyFSM>(out EnemyFSM enemy)) {
            enemy.TakeDamage(_data.throwDamage, _rb.velocity);
        }
    }

    public void ExitState()
    {
        // Reset throw direction
        _fsm.throwDirection = Vector2.zero;
    }
}
