using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJump : MonoBehaviour {
    private Rigidbody2D rb;
    private PlayerAnimations anim;
    private PlayerPlatformCollision collision;
    private PlayerCombat combat;
    private PlayerMovement movement;
    [SerializeField] private Vector2 jumpDirection;
    [SerializeField] private float movementDisableTime;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<PlayerAnimations>();
        combat = GetComponentInParent<PlayerCombat>();
        movement = GetComponentInParent<PlayerMovement>();
        Transform collTransform = transform.parent.Find("Platform Collision");
        if (collTransform)
            collision = collTransform.GetComponent<PlayerPlatformCollision>();
    }

    private void Update() {
        if (Input.GetButtonDown ("Jump")) {
            if (!collision.onGround && (collision.onLeftWall && (Input.GetAxisRaw("Horizontal") < 0) ||
                 (collision.onRightWall && (Input.GetAxisRaw("Horizontal") > 0)))) {
                StartCoroutine(movement.DisableMovement(movementDisableTime));
                StartCoroutine(Common.ChangeVariableAfterDelay<bool>(e => GetComponent<PlayerWallSlide>().canSlide = e, movementDisableTime, false, true));
                WallJump(collision.onLeftWall);
            }
        }
    }

    private void WallJump(bool isLeftWall) {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += new Vector2((isLeftWall ? 1 : -1) * jumpDirection.x, jumpDirection.y);
    }
}
