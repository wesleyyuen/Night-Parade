using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header ("References")]
    private Rigidbody2D rb;
    [SerializeField] private ParticleSystem dustTrail;
    [SerializeField] private PlayerPlatformCollision grounded;

    [Header ("Jumping Settings")]
    [SerializeField] private float jumpVelocity = 36f;
    [SerializeField] private float fallMultiplier = 0.8f;
    [SerializeField] private float lowJumpMultiplier = 5f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.05f;

    private float jumpBuffer;
    private float coyoteTimer;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update() {
        CoyoteAndJumpBuffering();
    }

    private void FixedUpdate() {
        if (jumpBuffer > 0 && coyoteTimer > 0) {
            CreateDustTrail ();
            jumpBuffer = 0;
            coyoteTimer = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpVelocity;
        }

        // Low Jump
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton ("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void CoyoteAndJumpBuffering() {
        // Coyote Time - allow late-input of jumps after touching the ground
        coyoteTimer -= Time.deltaTime;
        if (FindObjectOfType<PlayerPlatformCollision> ().onGround) {
            coyoteTimer = coyoteTime;
        }

        // Jump Buffering - allow pre-input of jumps before touching the ground
        jumpBuffer -= Time.deltaTime;
        Transform collTransform = transform.parent.Find("Platform Collision");
        if (Input.GetButtonDown ("Jump") && collTransform.GetComponent<PlayerPlatformCollision>().onGround) {
            jumpBuffer = jumpBufferTime;
        }
    }

    private void CreateDustTrail () {
        if (grounded.onGround) {
            dustTrail.Play ();
        }
    }
}
