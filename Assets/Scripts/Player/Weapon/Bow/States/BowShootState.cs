using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public sealed class BowShootState : IWeaponState, IBindInput
{
    private BowFSM _fsm;
    private PlayerAnimations _playerAnimation;
    private bool _isShootingRight = true;

    public BowShootState(BowFSM fsm)
    {
        _fsm = fsm;

        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void BindInput()
    {
    }

    public void UnbindInput()
    {
    }
 
    public void EnterState()
    {
        _isShootingRight = _playerAnimation.IsFacingRight();

        // play animation
        _playerAnimation.SetThrowAnimation();

        // TODO: should be called from player animation
        Shoot();
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }

    private void Shoot()
    {
        _fsm.isShotRight = _isShootingRight;

        // Shoot Arrow
        Vector3 offset = new Vector3(0f, 2f, 0f);
        GameObject arrowInstance = Object.Instantiate(_fsm.arrowPrefab, _fsm.transform.position + offset, Quaternion.identity);
        Rigidbody2D arrowRigibody = arrowInstance.GetComponent<Rigidbody2D>();

        arrowRigibody.velocity = (_isShootingRight ? Vector2.right : Vector2.left) * 70f;
    }

    public void ExitState()
    {
    }
}
