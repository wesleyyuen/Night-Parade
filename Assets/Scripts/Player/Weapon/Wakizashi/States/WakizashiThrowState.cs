using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class WakizashiThrowState : IWeaponState, IBindInput
{
    private WakizashiFSM _fsm;
    private WakizashiData _data;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerMovement _playerMovement;
    private PlayerAnimations _playerAnimation;
    private Vector2 _throwDirection = new Vector2(1.0f, 0.1f);
    private float _timer;
    private bool _isThrowingRight = true;
    private bool _isReturning, _stopUpdating;

    public WakizashiThrowState(WakizashiFSM fsm)
    {
        _fsm = fsm;
        _data = (WakizashiData)fsm.weaponData;

        _throwDirection.Normalize();
        _rb = fsm.GetComponent<Rigidbody2D>();
        _collider = fsm.GetComponent<Collider2D>();
        _playerMovement = fsm.GetComponentInParent<PlayerMovement>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void BindInput()
    {
        // _fsm.InputActions.Player.Attack.started += OnStartTeleport;
        _fsm.InputActions.Player.Throw.performed += OnStartReturn;
        _fsm.InputActions.Player.Throw.canceled += OnStartReturn;
    }

    public void UnbindInput()
    {
        // _fsm.InputActions.Player.Attack.started -= OnStartTeleport;
        _fsm.InputActions.Player.Throw.performed -= OnStartReturn;
        _fsm.InputActions.Player.Throw.canceled -= OnStartReturn;
    }
 
    public void EnterState()
    {
        _isReturning = false;
        _stopUpdating = false;
        _timer = 0f;

        _fsm.isThrownRight = _isThrowingRight = _playerAnimation.IsFacingRight();
        _fsm.throwCooldownTimer = _data.throwCooldown;

        // play animation
        Utility.FreezePlayer(true);
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
        if (!_isReturning)
            _isReturning = true;
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        // TODO: replace by animation (rotate in x/y axis mostly)
        _rb.transform.localEulerAngles += (_isThrowingRight ? Vector3.back : Vector3.forward) * 2700f * Time.deltaTime; 

        if (_timer < _data.throwMinDuration) {
            _rb.velocity = (_isThrowingRight ? Vector2.right : Vector2.left) * _data.throwForce;
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
            enemy.TakeDamage(_data.throwDamage);
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
    }
}
