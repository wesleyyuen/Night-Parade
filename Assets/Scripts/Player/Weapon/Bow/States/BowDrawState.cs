using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class BowDrawState : IWeaponState, IBindInput
{
    private BowFSM _fsm;
    private PlayerAnimations _playerAnimation;
    private bool _isShootingRight = true;
    private bool _releasedArrow;
    private bool _isCanceled;

    public BowDrawState(BowFSM fsm)
    {
        _fsm = fsm;

        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Shoot.performed += OnReleaseArrow;
        _fsm.InputActions.Player.Shoot.canceled += OnCancelDraw;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Shoot.performed -= OnReleaseArrow;
        _fsm.InputActions.Player.Shoot.canceled -= OnCancelDraw;
    }
 
    public void EnterState()
    {
        _releasedArrow = false;
        _isCanceled = false;
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
        if (_releasedArrow) {
            _releasedArrow = false;

            _isShootingRight = _playerAnimation.IsFacingRight();
            _fsm.isShotRight = _isShootingRight;

            // Shoot Arrow
            Vector3 offset = new Vector3(0f, 1f, 0f);
            GameObject arrowInstance = Object.Instantiate(_fsm.arrowPrefab, _fsm.transform.position + offset, Quaternion.identity);
            if (arrowInstance.TryGetComponent<Arrow>(out Arrow arrow)) {
                arrow.Shoot(_isShootingRight ? Vector2.right : Vector2.left);
            }

            _fsm.SetState(_fsm.states[BowStateType.Nock]);
        } else if (_isCanceled) {
            _fsm.SetState(_fsm.states[BowStateType.Idle]);
        }
    }

    private void OnReleaseArrow(InputAction.CallbackContext context)
    {
        _releasedArrow = true;
    }

    private void OnCancelDraw(InputAction.CallbackContext context)
    {
        _isCanceled = true;
    }

    public void ExitState()
    {
    }
}
