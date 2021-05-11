using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlide : MonoBehaviour {
    private Rigidbody2D rb;
    private PlayerAnimations anim;
    private PlayerPlatformCollision collision;
    private PlayerCombat combat;
    [SerializeField] private float slideSpeed;
    [HideInInspector] public bool canSlide;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<PlayerAnimations>();
        combat = GetComponentInParent<PlayerCombat>();
        Transform collTransform = transform.parent.Find("Platform Collision");
        if (collTransform)
            collision = collTransform.GetComponent<PlayerPlatformCollision>();
    }

    private void Update() {
        if (!canSlide) return;

        // Only disable attack if player is wallsliding
        combat.canAttack = true;

        if (!collision.onGround && collision.onWall) {
            // press against wall, slow down sliding
            if ((collision.onLeftWall && (Input.GetAxisRaw("Horizontal") < 0)) ||
                (collision.onRightWall && (Input.GetAxisRaw("Horizontal") > 0))) {
                combat.canAttack = false;
                WallSlide(slideSpeed);
            }
        }
    }

    private void WallSlide(float speed) {
        rb.velocity = new Vector2(rb.velocity.x, -speed);
    }
}
