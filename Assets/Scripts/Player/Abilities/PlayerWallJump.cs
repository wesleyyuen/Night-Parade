using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using Zenject;

public class PlayerWallJump : MonoBehaviour
{
    private InputManager _inputManager;
    private Rigidbody2D _rb;
    private PlayerPlatformCollision _collision;
    private PlayerMovement _movement;
    private PlayerAnimations _animations;
    private PlayerWallSlide _wallSlide;
    [SerializeField] private Vector2 _jumpDirection;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _movementDisableTime;

    [Inject]
    public void Initialize(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

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
        _inputManager.Event_GameplayInput_Jump += OnWallJump;
    }

    private void OnDisable()
    {
        _inputManager.Event_GameplayInput_Jump -= OnWallJump;
    }

    private void OnWallJump()
    {
        if (!_collision.onGround) {
            if ((_collision.onLeftWall && _inputManager.HasDirectionalInput(InputManager.DirectionInput.Left))
             || (_collision.onRightWall && _inputManager.HasDirectionalInput(InputManager.DirectionInput.Right))) {
                _rb.isKinematic = false;
                _movement.HandicapMovementForSeconds(_movementDisableTime);
                _animations.EnablePlayerTurning(false, _movementDisableTime);
                // Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _wallSlide.canSlide = e, _movementDisableTime, false, true));
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
