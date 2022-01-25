using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;

public sealed class WakizashiReturnState : IWeaponState
{
    private const float RETURN_DURATION_PER_UNIT = 0.03f;
    private const float RETURN_DISTANCE = 1f;
    private WakizashiFSM _fsm;
    private WakizashiData _data;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerAnimations _playerAnimation;
    private Vector2 _weaponEndPosition;
    private bool _isReturningRight;
    private bool _isOnEnemy;
    private Rigidbody2D _target;
    private Vector2 _originalPosition;
    private float _lerpTime;
    private bool _stopUpdating;

    public WakizashiReturnState(WakizashiFSM fsm)
    {
        _fsm = fsm;
        _data = (WakizashiData)fsm.weaponData;

        _rb = fsm.GetComponent<Rigidbody2D>();
        _collider = fsm.GetComponent<Collider2D>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }
 
    public void EnterState()
    {
        _stopUpdating = false;
        _isOnEnemy = _fsm.transform.parent?.gameObject.layer ==  LayerMask.NameToLayer("Enemies");
        _weaponEndPosition = _isOnEnemy ? (_fsm.player.position + new Vector2(_playerAnimation.IsFacingRight() ? 4f : -4f, 0f)) : (_fsm.player.position + new Vector2(0f, 1f));
        _isReturningRight = _fsm.transform.position.x >= _weaponEndPosition.x;
        _originalPosition = _fsm.transform.position;

        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
        _collider.enabled = true;

        _target = _isOnEnemy ? _fsm.transform.parent.GetComponent<Rigidbody2D>() : _rb;
        _lerpTime = 0f;
        float dist = Vector2.Distance(_weaponEndPosition, _target.position);
        DOTween.To(() => _lerpTime, x => _lerpTime = x, 1f, RETURN_DURATION_PER_UNIT * dist).SetEase(Ease.InSine);
    }

    private void OnReturnComplete()
    {
        _stopUpdating = true;

        CameraShake.Instance.ShakeCamera(0.2f, 0.1f);

        _fsm.transform.parent = _fsm.player.transform;
        _fsm.transform.position = Vector3.zero;
        Utility.FreezePlayer(false);
        _playerAnimation.SetUnlodgedAnimation();
        
        _fsm.SetState(_fsm.states[WakizashiStateType.Idle]);
    }

    public void Update()
    {
        if (_stopUpdating) return;

        _weaponEndPosition = _isOnEnemy ? (_fsm.player.position + new Vector2(_playerAnimation.IsFacingRight() ? 4f : -4f, 0f)) : (_fsm.player.position + new Vector2(0f, 1f));
        _target.position = (Vector3) Vector2.Lerp(_originalPosition, _weaponEndPosition, _lerpTime);

        // Return duration up or Close to player
        if (_lerpTime >= 1f || Vector2.Distance(_rb.position, _fsm.player.position) <= RETURN_DISTANCE) {
            OnReturnComplete();
        }

        // Rotate
        if (!_stopUpdating && !_isOnEnemy)
            _rb.transform.localEulerAngles += (_isReturningRight ? Vector3.forward : Vector3.back) * 2700f * Time.deltaTime;
    }

    public void FixedUpdate()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        // Only damage enemies if not already on enemy
        if (!_isOnEnemy && collider.gameObject.TryGetComponent<EnemyFSM>(out EnemyFSM enemy)) {
            enemy.TakeDamage(_data.throwDamage, _rb.velocity);
        }
    }

    public void ExitState()
    {
    }
}
