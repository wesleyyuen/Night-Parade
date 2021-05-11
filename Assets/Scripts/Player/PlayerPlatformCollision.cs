using UnityEngine;

public class PlayerPlatformCollision : MonoBehaviour {  
    private PlayerAudio playerAudio;
    [SerializeField] private bool startOffGrounded;
    [SerializeField] private float colliderRadius;
    [SerializeField] private PhysicsMaterial2D oneFriction;

    public bool onGround { get; private set; }
    public bool onSlope { get; private set; }
    public Vector2 slopeVector { get; private set; }
    public bool onWall { get; private set; }
    public bool onLeftWall { get; private set; }
    public bool onRightWall { get; private set; }
    private PlayerMovement movement;
    private PlayerAnimations animations;
    private Collider2D coll;
    private Rigidbody2D rb;
    private PlayerJump jump;
    private Vector2 groundDetectionPoint;
    private Vector2 leftDetectionPoint;
    private Vector2 rightDetectionPoint;

    private float lastYVelocity;

    private void Awake () {
        playerAudio = FindObjectOfType<PlayerAudio> ();
        coll = GetComponentInParent<Collider2D>();
        rb = GetComponentInParent<Rigidbody2D>();
        movement = GetComponentInParent<PlayerMovement>();
        animations = GetComponentInParent<PlayerAnimations>();
        jump = transform.parent.GetComponentInChildren<PlayerJump>();

        if (startOffGrounded)
            onGround = true;
    }

    private void Update() {
        bool prevOnGround = onGround;
        bool prevOnSlope = onSlope;
        bool prevOnLeftWall = onLeftWall;
        bool prevOnRightWall = onRightWall;
        bool prevOnWall = onWall;

        groundDetectionPoint = new Vector2 (coll.bounds.center.x, coll.bounds.min.y);
        leftDetectionPoint = new Vector2 (coll.bounds.min.x, coll.bounds.center.y);
        rightDetectionPoint = new Vector2 (coll.bounds.max.x, coll.bounds.center.y);

        Collider2D groundHit = Physics2D.OverlapCircle(groundDetectionPoint, colliderRadius, LayerMask.GetMask("Ground"));
        onGround = groundHit;
        onSlope = groundHit ? groundHit.CompareTag("Slope") : false;
        if (onSlope) {
            RaycastHit2D hit = Physics2D.Raycast(groundDetectionPoint, Vector2.down, colliderRadius, LayerMask.GetMask("Ground"));
            if (hit && hit.collider.CompareTag("Slope")) {
                slopeVector = Vector2.Perpendicular(hit.normal).normalized;
            }
        } else {
            slopeVector = Vector2.left;
        }
        onLeftWall = Physics2D.Raycast(leftDetectionPoint, Vector2.left, colliderRadius, LayerMask.GetMask("Wall"));
        //Physics2D.OverlapCircle(leftDetectionPoint, colliderRadius, LayerMask.GetMask("Wall"));
        onRightWall = Physics2D.Raycast(rightDetectionPoint, -Vector2.left, colliderRadius, LayerMask.GetMask("Wall"));
        //Physics2D.OverlapCircle(rightDetectionPoint, colliderRadius, LayerMask.GetMask("Wall"));
        onWall = onLeftWall || onRightWall;

        // onGround callbacks
        if (!prevOnGround && onGround) {
            OnGroundEnter();

        } else if (prevOnGround && !onGround) {
            OnGroundExit();
        }

        // Handle Slope
        if (onSlope)
            groundHit.sharedMaterial = Input.GetAxisRaw("Horizontal") == 0 ? oneFriction : null;

        lastYVelocity = rb.velocity.y;
    }

    private void OnGroundEnter() {
        playerAudio.FootstepSFX();
        animations.CreateDustTrail();

        // Big Fall
        const float bigFallHandicapDuration = 1f;
        const float bigFallVelocity = -40;
        if (lastYVelocity < bigFallVelocity) {
            movement.EnablePlayerMovement(false, bigFallHandicapDuration);
            animations.EnablePlayerTurning(false, bigFallHandicapDuration);
            jump.EnablePlayerJump(false, bigFallHandicapDuration);
            CameraShake.Instance.ShakeCamera(0.5f, bigFallHandicapDuration * 0.25f);
        }
    }

    private void OnGroundExit() {

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(groundDetectionPoint, Vector2.down * colliderRadius);
        Gizmos.DrawRay(leftDetectionPoint,  Vector2.left * colliderRadius);
        Gizmos.DrawRay(rightDetectionPoint,  -Vector2.left * colliderRadius);
    }
}