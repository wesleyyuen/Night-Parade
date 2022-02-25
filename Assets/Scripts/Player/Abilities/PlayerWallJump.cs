using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerWallJump : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerPlatformCollision _collision;
    PlayerMovement _movement;
    PlayerAnimations _animations;
    PlayerWallSlide _wallSlide;
    [SerializeField] Vector2 _jumpDirection;
    [SerializeField] float _jumpForce;
    [SerializeField] float _movementDisableTime;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _movement = GetComponentInParent<PlayerMovement>();
        _animations = GetComponentInParent<PlayerAnimations>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
        _wallSlide = GetComponent<PlayerWallSlide>();

        _jumpDirection = _jumpDirection.normalized;
    }

    private void OnEnable()
    {
        InputManager.Instance.Event_GameplayInput_Jump += OnWallJump;
    }

    private void OnDisable()
    {
        InputManager.Instance.Event_GameplayInput_Jump -= OnWallJump;
    }

    private void OnWallJump()
    {
        if (!_collision.onGround) {
            if ((_collision.onLeftWall && InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Left))
             || (_collision.onRightWall && InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Right))) {
                _rb.isKinematic = false;
                _movement.HandicapMovementForSeconds(_movementDisableTime);
                _animations.EnablePlayerTurning(false, _movementDisableTime);
                Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _wallSlide.canSlide = e, _movementDisableTime, false, true));
                WallJump(_collision.onLeftWall);
            }
        }
    }

    private void WallJump(bool isLeftWall)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.velocity += new Vector2((isLeftWall ? 1 : -1) * _jumpDirection.x * _jumpForce, _jumpDirection.y * _jumpForce);
        
        // Turn away from the wall
        _animations.FaceRight(isLeftWall);
    }
}
