using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class BowDrawState : IWeaponState, IBindInput
{
    private BowFSM _fsm;
    private BowData _data;
    private PlayerAnimations _playerAnimation;
    private bool _isShootingRight = true;
    private bool _releasedArrow;
    private bool _isCanceled;
    private bool _stopUpdating;

    public BowDrawState(BowFSM fsm)
    {
        _fsm = fsm;
        _data = (BowData)fsm.weaponData;

        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Shoot_SlowTap.performed += OnReleaseArrow;
        _fsm.InputActions.Player.Shoot_SlowTap.canceled += OnCancelDraw;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Shoot_SlowTap.performed -= OnReleaseArrow;
        _fsm.InputActions.Player.Shoot_SlowTap.canceled -= OnCancelDraw;
    }
 
    public void EnterState()
    {
        _releasedArrow = false;
        _isCanceled = false;
        _stopUpdating = false;

        Utility.FreezePlayer(true);
    }

    public void Update()
    {
        if (_stopUpdating) return;

        if (_releasedArrow) {
            _stopUpdating = true;

            // Reset Cooldown
            _fsm.drawArrowCooldownTimer = _data.drawArrowCooldown;
            _isShootingRight = _playerAnimation.IsFacingRight();
            _fsm.isShotRight = _isShootingRight;

            // Shoot Arrow
            Vector3 offset = new Vector3(0f, 1f, 0f);
            GameObject arrowInstance = Object.Instantiate(_fsm.arrowPrefab, _fsm.transform.position + offset, Quaternion.identity);
            arrowInstance.transform.localScale = new Vector2(_isShootingRight ? 1f : -1f, 1f);
            if (arrowInstance.TryGetComponent<Arrow>(out Arrow arrow)) {
                arrow.Shoot(_isShootingRight ? Vector2.right : Vector2.left);
            }

            _fsm.SetState(_fsm.states[BowStateType.Nock]);
        }
        else if (_isCanceled) {
            _stopUpdating = true;
            _fsm.SetState(_fsm.states[BowStateType.Idle]);
        }
    }

    public void FixedUpdate()
    {
    }

    private void OnReleaseArrow(InputAction.CallbackContext context)
    {
        if (!_releasedArrow)
            _releasedArrow = true;
    }

    private void OnCancelDraw(InputAction.CallbackContext context)
    {
        if (!_isCanceled)
            _isCanceled = true;
    }

    public void ExitState()
    {
        Utility.FreezePlayer(false);
    }
}
