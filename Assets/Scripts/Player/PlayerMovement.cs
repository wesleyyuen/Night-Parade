using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour {

    [Header ("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem dustTrail;
    [SerializeField] private Grounded grounded;
    [SerializeField] private AudioManager audioManager;

    [Header ("Movement Settings")]
    [SerializeField] private float movementSpeed = 11f;
    [SerializeField] private float jumpVelocity = 36f;
    [SerializeField] private float fallMultiplier = 0.8f;
    [SerializeField] private float lowJumpMultiplier = 5f;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float coyoteTime = 0.05f;
    [SerializeField] private float slopeRaycastLength;

    private float jumpBuffer;
    private float coyoteTimer;

    void Awake () {
        audioManager = FindObjectOfType<AudioManager> ();
    }

    void Update () {
        animator.SetBool ("IsGrounded", grounded.isGrounded);
        // Jump uses rb.velocity, should be in Update() instead of FixedUpdate()
        Jump ();
    }

    void FixedUpdate () {
        float horizontalInput = Input.GetAxisRaw ("Horizontal");
        Vector3 prevLocalScale = transform.localScale;

        if (horizontalInput > 0) {
            if (prevLocalScale.x != 1f) {
                CreateDustTrail ();
                animator.SetBool ("FacingRight", true);
                transform.localScale = new Vector3 (1f, 1f, 1f);
            }
        } else if (horizontalInput < 0) {
            if (prevLocalScale.x != -1f) {
                CreateDustTrail ();
                animator.SetBool ("FacingRight", false);
                transform.localScale = new Vector3 (-1f, 1f, 1f);
            }
        }

        animator.SetFloat ("Horizontal", horizontalInput);
        animator.SetFloat ("Vertical", rb.velocity.y);

        Vector2 horizontalMovement = new Vector2 (horizontalInput * movementSpeed, 0.0f);
        rb.position += horizontalMovement * Time.deltaTime;
    }

    void Jump () {
        // Coyote Time
        coyoteTimer -= Time.deltaTime;
        if (FindObjectOfType<Grounded> ().isGrounded) {
            coyoteTimer = coyoteTime;
        }

        // Jump Buffering
        jumpBuffer -= Time.deltaTime;
        if (Input.GetButtonDown ("Jump")) {
            jumpBuffer = jumpBufferTime;
        }

        if (jumpBuffer > 0 && coyoteTimer > 0) {
            CreateDustTrail ();
            jumpBuffer = 0;
            coyoteTimer = 0;
            rb.velocity += Vector2.up * jumpVelocity;
        }
        // Low Jump
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton ("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void CreateDustTrail () {
        if (grounded.isGrounded) {
            dustTrail.Play ();
        }
    }

    void FootstepSFX () {
        audioManager.Play ("Forest_Footsteps");
    }
}