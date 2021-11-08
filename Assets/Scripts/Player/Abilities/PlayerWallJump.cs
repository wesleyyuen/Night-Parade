using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallJump : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerAnimations _anim;
    PlayerPlatformCollision _collision;
    PlayerMovement _movement;
    PlayerAnimations _animations;
    PlayerWallSlide _wallSlide;
    InputMaster _input;
    [SerializeField] Vector2 jumpDirection;
    [SerializeField] float movementDisableTime;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _movement = GetComponentInParent<PlayerMovement>();
        _animations = GetComponentInParent<PlayerAnimations>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
        _wallSlide = GetComponent<PlayerWallSlide>();

        // Handle Input
        _input = new InputMaster();
        _input.Player.Movement.Enable();
        _input.Player.Jump.Enable();
        _input.Player.Jump.started += OnWallJump;
    }

    void OnWallJump(InputAction.CallbackContext context)
    {
        float xRaw = _input.Player.Movement.ReadValue<Vector2>().x;
        if (!_collision.onGround && (_collision.onLeftWall && (xRaw < 0) || (_collision.onRightWall && (xRaw > 0)))) {
            _movement.HandicapMovementForSeconds(movementDisableTime);
            _animations.EnablePlayerTurning(false, movementDisableTime);
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _wallSlide.canSlide = e, movementDisableTime, false, true));
            WallJump(_collision.onLeftWall);
        }
    }

    void WallJump(bool isLeftWall)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.velocity += new Vector2((isLeftWall ? 1 : -1) * jumpDirection.x, jumpDirection.y);
        
        // Turn away from the wall
        _animations.FaceRight(isLeftWall);
    }
}
