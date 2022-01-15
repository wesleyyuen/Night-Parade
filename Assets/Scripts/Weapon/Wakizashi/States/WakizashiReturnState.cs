using UnityEngine;

public sealed class WakizashiReturnState : IWeaponState
{
    private const float _returnTime = 0.3f;
    private WakizashiFSM _fsm;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerAnimations _playerAnimation;
    private Vector2 _weaponStartPosition, _weaponEndPosition, _curvePoint;
    private float _timer;
    private bool _isReturningRight;
    private bool _isOnEnemy;

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
        _isOnEnemy = _fsm.transform.parent?.gameObject.layer ==  LayerMask.NameToLayer("Enemies");
        _isReturningRight = _fsm.transform.position.x >= _fsm.player.position.x;
        _weaponStartPosition = _fsm.transform.position;

        _rb.isKinematic = true;
        _collider.enabled = true;
    }

      public void Update()
    {
        _weaponEndPosition = _isOnEnemy ? (_fsm.player.position + new Vector2(_playerAnimation.IsFacingRight() ? 4f : -4f, 0f)) : (_fsm.player.position + new Vector2(0f, 1f));
        // _curvePoint = _weaponEndPosition + new Vector2(_playerAnimation.IsFacingRight() ? 3f : -3f, _isOnEnemy ? 0f : 2f);
        _curvePoint = _weaponStartPosition;

        if (!_isOnEnemy)
            _rb.transform.localEulerAngles += (_isReturningRight ? Vector3.forward : Vector3.back) * 2700f * Time.deltaTime; 

        // Moving towards desired point
        if (_timer < 1f) {
            if (_isOnEnemy) {
                _fsm.transform.parent.position = GetQuadraticCurvePoint(_timer, _weaponStartPosition, _curvePoint, _weaponEndPosition);
            } else {
                _fsm.transform.position = GetQuadraticCurvePoint(_timer, _weaponStartPosition, _curvePoint, _weaponEndPosition);
            }
            _timer += Time.deltaTime / _returnTime;
        }
        // Reaches the player
        else {
            _fsm.transform.parent = _fsm.player.transform;
            Utility.FreezePlayer(false);
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

    public void ExitState()
    {
    }
}
