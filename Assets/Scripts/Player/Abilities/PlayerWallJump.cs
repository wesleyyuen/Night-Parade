using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerWallJump : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerAnimations _anim;
    PlayerPlatformCollision _collision;
    PlayerMovement _movement;
    PlayerAnimations _animations;
    PlayerWallSlide _wallSlide;
    [SerializeField] Vector2 _jumpDirection;
    [SerializeField] float _jumpForce;
    [SerializeField] float _movementDisableTime;

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _movement = GetComponentInParent<PlayerMovement>();
        _animations = GetComponentInParent<PlayerAnimations>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
        _wallSlide = GetComponent<PlayerWallSlide>();

        _jumpDirection = _jumpDirection.normalized;
    }

    void OnEnable()
    {
        InputManager.Event_Input_Jump += OnWallJump;
    }

    void OnDisable()
    {
        InputManager.Event_Input_Jump -= OnWallJump;
    }

    void OnWallJump()
    {
        if (!_collision.onGround) {
            if ((_collision.onLeftWall && InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Left))
             || (_collision.onRightWall && InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Right))) {
                 // TODO: add clinging for better control
                _movement.HandicapMovementForSeconds(_movementDisableTime);
                _animations.EnablePlayerTurning(false, _movementDisableTime);
                Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _wallSlide.canSlide = e, _movementDisableTime, false, true));
                WallJump(_collision.onLeftWall);
            }
        }
    }

    void WallJump(bool isLeftWall)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.velocity += new Vector2((isLeftWall ? 1 : -1) * _jumpDirection.x * _jumpForce, _jumpDirection.y * _jumpForce);
        
        // Turn away from the wall
        _animations.FaceRight(isLeftWall);
    }
}
