using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class WakizashiThrownState : IWeaponState, IBindInput
{
    private WakizashiFSM _fsm;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerAnimations _playerAnimation;
    private Vector2 _throwDirection = new Vector2(1.0f, 0.1f);
    private bool _isThrowingRight = true;
    private bool _isReturning, _stopUpdating;

    public WakizashiThrownState(WakizashiFSM fsm)
    {
        _fsm = fsm;

        _throwDirection.Normalize();
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

        _fsm.transform.localPosition = new Vector2(0f, 2f);
        _isThrowingRight = _playerAnimation.IsFacingRight();

        // play animation
        _playerAnimation.SetThrowAnimation();

        // TODO: should be called from player animation
        Throw();
    }

    private void OnStartReturn(InputAction.CallbackContext context)
    {
        if (!_isReturning)
            _isReturning = true;
    }

    public void Update()
    {
        // Rotate Wakizashi
        // TODO: replace by animation (rotate in x/y axis mostly)
        Vector2 dir = _rb.velocity.normalized;
        float angle = Vector2.SignedAngle(_isThrowingRight ? Vector2.right : Vector2.left, dir);
        _fsm.transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        // _rb.transform.localEulerAngles += (_isThrowingRight ? Vector3.back : Vector3.forward) * 2700f * Time.deltaTime; 

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
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collider.gameObject.layer == LayerMask.NameToLayer("Wall") ||
            collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            HandleHit(collider.gameObject);
    }

    private void Throw()
    {
        _rb.isKinematic = false;
        _collider.enabled = true;

        // Detach from player
        _fsm.transform.parent = null;

        _rb.velocity = Vector2.zero;
        _rb.AddForce(new Vector2(_isThrowingRight ? _throwDirection.x : -_throwDirection.x, _throwDirection.y) * _fsm.weaponData.throwForce, ForceMode2D.Impulse);
    }

    private void HandleHit(GameObject hit)
    {
        // Damage Enemy
        if (hit.gameObject.TryGetComponent<EnemyFSM>(out EnemyFSM enemy)) {
            enemy.TakeDamage(_fsm.weaponData.throwDamage);
        }

        // Lodge it in
        if (!_isReturning && !_stopUpdating) {
            _fsm.transform.parent = hit.gameObject.transform;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;

            _fsm.SetState(_fsm.states[WeaponFSM.StateType.LodgedState]);
        }
    }

    public void ExitState()
    {
    }
}
