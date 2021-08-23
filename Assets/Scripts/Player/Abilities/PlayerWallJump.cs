using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerAnimations _anim;
    PlayerPlatformCollision _collision;
    PlayerMovement _movement;
    PlayerAnimations _animations;
    [SerializeField] Vector2 jumpDirection;
    [SerializeField] float movementDisableTime;

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _movement = GetComponentInParent<PlayerMovement>();
        _animations = GetComponentInParent<PlayerAnimations>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
    }

    void Update()
    {
        if (!_collision.onGround && (_collision.onLeftWall && (Input.GetAxisRaw("Horizontal") < 0) ||
                (_collision.onRightWall && (Input.GetAxisRaw("Horizontal") > 0))) && Input.GetButtonDown ("Jump")) {
            StartCoroutine(_movement.HandicapMovement(movementDisableTime));
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _animations.canTurn = e, movementDisableTime, false, true));
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => GetComponent<PlayerWallSlide>().canSlide = e, movementDisableTime, false, true));
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
