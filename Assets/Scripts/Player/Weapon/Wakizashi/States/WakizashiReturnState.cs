using UnityEngine;

public sealed class WakizashiReturnState : IWeaponState
{
    private const float _returnTime = 0.3f;
    private WakizashiFSM _fsm;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerAnimations _playerAnimation;
    private Vector2 _weaponStartPosition;
    private Vector2 _curvePoint;
    private float _timer;
    private bool _isReturningRight;

    public WakizashiReturnState(WakizashiFSM fsm)
    {
        _fsm = fsm;

        _rb = fsm.GetComponent<Rigidbody2D>();
        _collider = fsm.GetComponent<Collider2D>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }
 
    public void EnterState()
    {
        _timer = 0;
        _isReturningRight = _fsm.transform.position.x >= _fsm.player.position.x;
        _weaponStartPosition = _fsm.transform.position;
        _curvePoint = _fsm.player.position + new Vector2(_playerAnimation.IsFacingRight() ? 2f : -2f, 3f);
        ReturnToPlayer();

        // play animation
    }

    public void Update()
    {
        _rb.transform.localEulerAngles += (_isReturningRight ? Vector3.forward : Vector3.back) * 2700f * Time.deltaTime; 

        if (_timer < 1f) {
            _fsm.transform.position = GetQuadraticCurvePoint(_timer, _weaponStartPosition, _curvePoint, _fsm.player.position + new Vector2(0f, 1f));
            _timer += Time.deltaTime / _returnTime;
        } else {
            _playerAnimation.SetUnlodgedAnimation();
            _fsm.SetState(_fsm.states[WakizashiStateType.Idle]);
        }
    }

    private Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }

    public void FixedUpdate()
    {
    }

    private void ReturnToPlayer()
    {
        _rb.isKinematic = true;
        _collider.enabled = true;

        // Reattach to player
        _fsm.transform.parent = _fsm.player.transform;
    }

    public void ExitState()
    {
    }
}
