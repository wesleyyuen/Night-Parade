using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class WakizashiThrowState : IWeaponState, IBindInput
{
    private WakizashiFSM _fsm;
    private WakizashiData _data;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerAnimations _playerAnimation;
    private float _timer;
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
        // _fsm.InputActions.Player.Attack.started += OnStartTeleport;
        _fsm.InputActions.Player.Throw_SlowTap.performed += OnStartReturn;
        _fsm.InputActions.Player.Throw_SlowTap.canceled += OnStartReturn;
    }

    public void UnbindInput()
    {
        // _fsm.InputActions.Player.Attack.started -= OnStartTeleport;
        _fsm.InputActions.Player.Throw_SlowTap.performed -= OnStartReturn;
        _fsm.InputActions.Player.Throw_SlowTap.canceled -= OnStartReturn;
    }
 
    public void EnterState()
    {
        _isReturning = false;
        _stopUpdating = false;
        _timer = 0f;

        if (_fsm.throwDirection == Vector2.zero) {
            _fsm.throwDirection = _playerAnimation.IsFacingRight() ? Vector2.right : Vector2.left;
        }
        
        _fsm.throwCooldownTimer = _data.throwCooldown;

        // play animation
        Utility.FreezePlayer(true, _data.throwMinDuration);
        _playerAnimation.SetThrowAnimation();

        // TODO: should be called from player animation
        _collider.enabled = true;

        // Detach from player
        _fsm.transform.localPosition = new Vector2(0f, 1.5f);
        _fsm.transform.parent = null;

        _rb.velocity = Vector2.zero;
    }

    private void OnStartReturn(InputAction.CallbackContext context)
    {
        if (_fsm.IsCurrentState(WakizashiStateType.Throw) && !_isReturning)
            _isReturning = true;
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        // TODO: replace by animation (rotate in x/y axis mostly)
        _rb.transform.localEulerAngles += (_fsm.throwDirection.x > 0f ? Vector3.back : Vector3.forward) * 2700f * Time.deltaTime; 

        if (_timer < _data.throwMinDuration) {
            _rb.velocity = _fsm.throwDirection * _data.throwVelocity;
        } else {
            _rb.velocity = Vector2.zero;
            // Return if thrown more than throwMaxDuration
            if (!_stopUpdating && (_isReturning || _timer >= _data.throwMaxDuration)) {
                _stopUpdating = true;
                _fsm.SetState(_fsm.states[WakizashiStateType.Return]);
            }
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
            enemy.TakeDamage(_data.throwDamage, _rb.velocity);
        }

        // Lodge it in
        if (!_isReturning && !_stopUpdating) {
            _fsm.transform.parent = hit.gameObject.transform;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;

            _fsm.SetState(_fsm.states[WakizashiStateType.Lodged]);
        }
    }

    public void ExitState()
    {
        _fsm.throwDirection = Vector2.zero;
    }
}
