using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header ("References")]
    private Rigidbody2D rb;
    [SerializeField] private ParticleSystem dustTrail;
    private PlayerPlatformCollision coll;

    [Header ("Jumping Settings")]
    [SerializeField] private float jumpVelocity = 36f;
    [SerializeField] private float fallMultiplier = 0.8f;
    [SerializeField] private float lowJumpMultiplier = 5f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.05f;
    public bool canJump {get; set;}
    private float jumpBuffer;
    private float coyoteTimer;

    private void Start() {
        rb = GetComponentInParent<Rigidbody2D>();
        coll = gameObject.transform.parent.gameObject.GetComponentInChildren<PlayerPlatformCollision>();
        canJump = true;
    }

    public void EnablePlayerJump(bool enabled, float time = 0f)
    {
        if (canJump == enabled) return;

        if (time == 0)
            canJump = enabled;
        else
            StartCoroutine(Common.ChangeVariableAfterDelay<bool>(e => canJump = e, time, enabled, !enabled));
    }

    private void Update() {
        if (!canJump) return;
        CoyoteAndJumpBuffering();
    }

    private void FixedUpdate() {
        if (!canJump) return;

        if (jumpBuffer > 0 && coyoteTimer > 0) {
            CreateDustTrail ();
            jumpBuffer = 0;
            coyoteTimer = 0;
            
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpVelocity;
            // rb.AddForce(new Vector2(rb.velocity.x, jumpVelocity), ForceMode2D.Impulse);
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
        if (Input.GetButtonDown ("Jump") && coll.onGround) {
            jumpBuffer = jumpBufferTime;
        }
    }

    private void CreateDustTrail () {
        if (coll.onGround) {
            dustTrail.Play ();
        }
    }
}
