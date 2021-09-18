using UnityEngine;

public class PlayerPlatformCollision : MonoBehaviour {  
    PlayerAudio _playerAudio;
    [SerializeField] bool startOffGrounded;
    [SerializeField] float colliderRadius;
    [SerializeField] PhysicsMaterial2D oneFriction;

    public bool onGround { get; private set; }
    public bool onSlope { get; private set; }
    public Vector2 slopeVector { get; private set; }
    public bool onWall { get; private set; }
    public bool onLeftWall { get; private set; }
    public bool onRightWall { get; private set; }
    PlayerMovement _movement;
    PlayerAnimations _animations;
    InputMaster _input;
    Collider2D _coll;
    Rigidbody2D _rb;
    PlayerJump _jump;
    Vector2 _groundDetectionPoint,
            _leftWallUpperDetectionPoint, _leftWallMidDetectionPoint,_leftWallLowerDetectionPoint,
            _rightWallUpperDetectionPoint, _rightWallMidDetectionPoint, _rightWallLowerDetectionPoint;

    int _groundLayerMask, _wallLayerMask;

    float lastYVelocity;

    void Awake () {
        _playerAudio = FindObjectOfType<PlayerAudio> ();
        _coll = GetComponentInParent<Collider2D>();
        _rb = GetComponentInParent<Rigidbody2D>();
        _movement = GetComponentInParent<PlayerMovement>();
        _animations = GetComponentInParent<PlayerAnimations>();
        _jump = transform.parent.GetComponentInChildren<PlayerJump>();
        _groundLayerMask = LayerMask.GetMask("Ground");
        _wallLayerMask = LayerMask.GetMask("Wall");

        if (startOffGrounded)
            onGround = true;

        // Handle Inputs
        _input = new InputMaster();
        _input.Player.Movement.Enable();
    }

    void Update() {
        bool prevOnGround = onGround;
        bool prevOnSlope = onSlope;
        bool prevOnLeftWall = onLeftWall;
        bool prevOnRightWall = onRightWall;
        bool prevOnWall = onWall;

        _groundDetectionPoint = new Vector2 (_coll.bounds.center.x, _coll.bounds.min.y);
        _leftWallUpperDetectionPoint = new Vector2 (_coll.bounds.min.x, _coll.bounds.max.y);
        _leftWallMidDetectionPoint = new Vector2 (_coll.bounds.min.x, _coll.bounds.center.y);
        _leftWallLowerDetectionPoint = new Vector2 (_coll.bounds.min.x, _coll.bounds.min.y);
        _rightWallUpperDetectionPoint = new Vector2 (_coll.bounds.max.x, _coll.bounds.max.y);
        _rightWallMidDetectionPoint = new Vector2 (_coll.bounds.max.x, _coll.bounds.center.y);
        _rightWallLowerDetectionPoint = new Vector2 (_coll.bounds.max.x, _coll.bounds.min.y);

        Collider2D groundHit = Physics2D.OverlapCircle(_groundDetectionPoint, colliderRadius, _groundLayerMask);
        onGround = groundHit;
        onSlope = groundHit ? groundHit.CompareTag("Slope") : false;
        if (onSlope) {
            RaycastHit2D hit = Physics2D.Raycast(_groundDetectionPoint, Vector2.down, colliderRadius, _groundLayerMask);
            if (hit && hit.collider.CompareTag("Slope")) {
                slopeVector = Vector2.Perpendicular(hit.normal).normalized;
            }
        } else {
            slopeVector = Vector2.left;
        }
        onLeftWall = Physics2D.Raycast(_leftWallUpperDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask) ||
                     Physics2D.Raycast(_leftWallMidDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask) ||
                     Physics2D.Raycast(_leftWallLowerDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask);
        onRightWall = Physics2D.Raycast(_rightWallUpperDetectionPoint, -Vector2.left, colliderRadius, _wallLayerMask) ||
                      Physics2D.Raycast(_rightWallMidDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask) ||
                      Physics2D.Raycast(_rightWallLowerDetectionPoint, -Vector2.left, colliderRadius, _wallLayerMask);
        onWall = onLeftWall || onRightWall;

        // onGround callbacks
        if (!prevOnGround && onGround) {
            OnGroundEnter();

        } else if (prevOnGround && !onGround) {
            OnGroundExit();
        }

        // Handle Slope
        if (onSlope)
            groundHit.sharedMaterial = _input.Player.Movement.ReadValue<Vector2>().x == 0 ? oneFriction : null;

        lastYVelocity = _rb.velocity.y;
    }

    void OnGroundEnter() {
        _playerAudio.PlayFootstepSFX();
        _animations.CreateDustTrail();

        // Big Fall
        const float bigFallHandicapDuration = 1f;
        const float bigFallVelocity = -40;
        if (lastYVelocity < bigFallVelocity) {
            _movement.EnablePlayerMovement(false, bigFallHandicapDuration);
            _animations.EnablePlayerTurning(false, bigFallHandicapDuration);
            _jump.EnablePlayerJump(false, bigFallHandicapDuration);
            CameraShake.Instance.ShakeCamera(0.5f, bigFallHandicapDuration * 0.25f);
        }
    }

    void OnGroundExit() {
        // TODO: not working
        // animations.CreateDustTrail();
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(_groundDetectionPoint, Vector2.down * colliderRadius);
        Gizmos.DrawRay(_leftWallUpperDetectionPoint,  Vector2.left * colliderRadius);
        Gizmos.DrawRay(_leftWallLowerDetectionPoint,  -Vector2.left * colliderRadius);
        Gizmos.DrawRay(_rightWallUpperDetectionPoint,  Vector2.left * colliderRadius);
        Gizmos.DrawRay(_rightWallLowerDetectionPoint,  -Vector2.left * colliderRadius);
    }
}