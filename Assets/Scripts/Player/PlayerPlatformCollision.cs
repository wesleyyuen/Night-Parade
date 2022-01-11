using UnityEngine;

public class PlayerPlatformCollision : MonoBehaviour
{  
    public bool onGround { get; private set; }
    public event System.Action Event_OnGroundEnter;
    // public bool onSlope { get; private set; }
    // public Vector2 slopeVector { get; private set; }
    // public Vector2 slopeNormal { get; private set; }
    public bool onWall { get; private set; }
    public bool onLeftWall { get; private set; }
    public bool onRightWall { get; private set; }
    [SerializeField] private float colliderRadius;
    [SerializeField] private PhysicsMaterial2D _oneFriction;
    private PhysicsMaterial2D _originalMaterial;
    private PlayerMovement _movement;
    private PlayerAnimations _animations;
    private PlayerAudio _playerAudio;
    private Collider2D _coll;
    private Rigidbody2D _rb;
    private PlayerJump _jump;
    private Vector2 _groundDetectionPoint,
            _leftWallUpperDetectionPoint, _leftWallMidDetectionPoint,_leftWallLowerDetectionPoint,
            _rightWallUpperDetectionPoint, _rightWallMidDetectionPoint, _rightWallLowerDetectionPoint;

    private int _groundLayerMask, _wallLayerMask;
    private float _lastYVelocity, _fallStartYPos;
    private bool _startedFalling;

    private void Awake()
    {
        _playerAudio = FindObjectOfType<PlayerAudio> ();
        _coll = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _movement = GetComponent<PlayerMovement>();
        _animations = GetComponent<PlayerAnimations>();
        _jump = transform.parent.GetComponentInChildren<PlayerJump>();
        _originalMaterial = _rb.sharedMaterial;
        _groundLayerMask = LayerMask.GetMask("Ground");
        _wallLayerMask = LayerMask.GetMask("Wall");

        onGround = true;
    }

    private void FixedUpdate()
    {
        bool prevOnGround = onGround;
        // bool prevOnSlope = onSlope;
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

        // Make ground detection Box narrower than player hitbox to avoid detecting ground next to player
        Collider2D groundHit = Physics2D.OverlapArea(_groundDetectionPoint - new Vector2(_coll.bounds.size.x * 0.95f /2f, colliderRadius),
                              _groundDetectionPoint + new Vector2(_coll.bounds.size.x * 0.95f /2f, colliderRadius),
                              _groundLayerMask);
        onGround = groundHit;

        // Handle Slope
        // onSlope = groundHit;
        // slopeVector = _animations.IsFacingRight() ? -Vector2.left : Vector2.left;
        // slopeNormal = Vector2.up;
        // if (onGround) {
        //     RaycastHit2D raycast = Physics2D.Raycast(_groundDetectionPoint, Vector2.down, 0.5f, _groundLayerMask);
        //     slopeNormal = raycast.normal;
        //     slopeVector = _animations.IsFacingRight() ? -Vector2.Perpendicular(raycast.normal) : Vector2.Perpendicular(raycast.normal);
        //     onSlope = Vector2.Angle(raycast.normal, Vector2.up) > 0f;
        // }
        // slopeVector.Normalize();
        // slopeNormal.Normalize();
        

        onLeftWall = Physics2D.Raycast(_leftWallUpperDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask) ||
                     Physics2D.Raycast(_leftWallMidDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask) ||
                     Physics2D.Raycast(_leftWallLowerDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask);
        onRightWall = Physics2D.Raycast(_rightWallUpperDetectionPoint, -Vector2.left, colliderRadius, _wallLayerMask) ||
                      Physics2D.Raycast(_rightWallMidDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask) ||
                      Physics2D.Raycast(_rightWallLowerDetectionPoint, -Vector2.left, colliderRadius, _wallLayerMask);
        onWall = onLeftWall || onRightWall;

        // Ground Callbacks
        if (!prevOnGround && onGround)
            OnGroundEnter();
        else if (prevOnGround && !onGround)
            OnGroundExit();

        // Slope Callbacks
        // if (!prevOnSlope && onSlope)
        //     OnSlopeEnter();
        // else if (prevOnSlope && !onSlope)
        //     OnSlopeExit();

        // Wall Callbacks
        // if (!prevOnWall && onWall)
        //     OnWallEnter();
        // else if (prevOnWall && !onWall)
        //     OnWallExit();

        // Starts falling
        if (!_startedFalling && !onGround && _lastYVelocity >= 0f && _rb.velocity.y < 0) {
            UpdateFallPosition();
            _startedFalling = true;
        }
        _lastYVelocity = _rb.velocity.y;
    }
    
    private void OnGroundEnter()
    {
        Event_OnGroundEnter?.Invoke();
        
        // Handle Big Fall
        const float bigFallHeight = 20f;
        float currFallHeight = _fallStartYPos - _rb.position.y;
        if (currFallHeight > bigFallHeight)
            OnBigFall();

        _startedFalling = false;
        UpdateFallPosition();

        // Squish player depending on height of the fall
        const float jumpHeight = 8f;
        float squishDuration = Mathf.Lerp(0.1f, 0.15f, currFallHeight / jumpHeight);
        float squishXScale = Mathf.Lerp(1.05f, 1.2f, currFallHeight / jumpHeight);
        float squishYScale = Mathf.Lerp(0.95f, 0.75f, currFallHeight / jumpHeight);
        _animations.Squish(squishDuration, new Vector2((_animations.IsFacingRight() ? 1f : -1f) * squishXScale, squishYScale));

        // Landing Sound
        _playerAudio.PlayLandingSFX();
    }

    private void OnGroundExit()
    {
        // _animations.CreateDustTrail();
        _startedFalling = false;
        UpdateFallPosition();
    }

    // void OnSlopeEnter()
    // {
    //     _rb.sharedMaterial = _oneFriction;
    // }

    // void OnSlopeExit()
    // {
    //     _rb.sharedMaterial = _originalMaterial;
    // }

    private void OnBigFall()
    {
        const float bigFallHandicapDuration = 1f;
        _movement.EnablePlayerMovement(false, bigFallHandicapDuration);
        _animations.EnablePlayerTurning(false, bigFallHandicapDuration);
        _animations.SetRunAnimation(0f);
        _jump.EnablePlayerJump(false, bigFallHandicapDuration);
        CameraShake.Instance.ShakeCamera(0.5f, bigFallHandicapDuration * 0.25f);
    }

    // void OnWallEnter()
    // {
    // }

    // void OnWallExit()
    // {
    // }

    public void UpdateFallPosition()
    {
        _fallStartYPos = _rb.position.y;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        // Gizmos.DrawCube(_groundDetectionPoint, new Vector3(_coll.bounds.size.x * 0.95f, colliderRadius, 0f));
        Gizmos.DrawRay(_leftWallUpperDetectionPoint,  Vector2.left * colliderRadius);
        Gizmos.DrawRay(_leftWallLowerDetectionPoint,  -Vector2.left * colliderRadius);
        Gizmos.DrawRay(_rightWallUpperDetectionPoint,  Vector2.left * colliderRadius);
        Gizmos.DrawRay(_rightWallLowerDetectionPoint,  -Vector2.left * colliderRadius);
    }
}