using UnityEngine;

public class PlayerPlatformCollision : MonoBehaviour
{  
    [SerializeField] float colliderRadius;
    public bool onGround { get; private set; }
    public bool onSlope { get; private set; }
    public Vector2 slopeVector { get; private set; }
    public bool onWall { get; private set; }
    public bool onLeftWall { get; private set; }
    public bool onRightWall { get; private set; }
    PlayerMovement _movement;
    PlayerAnimations _animations;
    PlayerAudio _playerAudio;
    Collider2D _coll;
    Rigidbody2D _rb;
    PlayerJump _jump;
    Vector2 _groundDetectionPoint,
            _leftWallUpperDetectionPoint, _leftWallMidDetectionPoint,_leftWallLowerDetectionPoint,
            _rightWallUpperDetectionPoint, _rightWallMidDetectionPoint, _rightWallLowerDetectionPoint;

    int _groundLayerMask, _wallLayerMask;
    float _lastYVelocity, _fallStartYPos;
    bool _startedFalling;

    void Start()
    {
        _playerAudio = FindObjectOfType<PlayerAudio> ();
        _coll = GetComponentInParent<Collider2D>();
        _rb = GetComponentInParent<Rigidbody2D>();
        _movement = GetComponentInParent<PlayerMovement>();
        _animations = GetComponentInParent<PlayerAnimations>();
        _jump = transform.parent.GetComponentInChildren<PlayerJump>();
        _groundLayerMask = LayerMask.GetMask("Ground");
        _wallLayerMask = LayerMask.GetMask("Wall");

        onGround = true;
    }

    void Update()
    {
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

        // Make ground detection Box narrower than player hitbox to avoid detecting ground next to player
        Collider2D groundHit = Physics2D.OverlapArea(_groundDetectionPoint - new Vector2(_coll.bounds.size.x * 0.95f /2f, colliderRadius),
                              _groundDetectionPoint + new Vector2(_coll.bounds.size.x * 0.95f /2f, colliderRadius),
                              _groundLayerMask);
        onGround = groundHit;
        slopeVector = _animations.GetPlayerScale().x > 0 ? -Vector2.left : Vector2.left;
        if (onGround) {
            RaycastHit2D raycast = Physics2D.Raycast(_groundDetectionPoint, Vector2.down, colliderRadius, _groundLayerMask);
            slopeVector = _animations.GetPlayerScale().x > 0 ? -Vector2.Perpendicular(raycast.normal) : Vector2.Perpendicular(raycast.normal);
            onSlope = Vector2.Angle(raycast.normal, Vector2.up) > 0f;
        }
        slopeVector = slopeVector.normalized;

        onLeftWall = Physics2D.Raycast(_leftWallUpperDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask) ||
                     Physics2D.Raycast(_leftWallMidDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask) ||
                     Physics2D.Raycast(_leftWallLowerDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask);
        onRightWall = Physics2D.Raycast(_rightWallUpperDetectionPoint, -Vector2.left, colliderRadius, _wallLayerMask) ||
                      Physics2D.Raycast(_rightWallMidDetectionPoint, Vector2.left, colliderRadius, _wallLayerMask) ||
                      Physics2D.Raycast(_rightWallLowerDetectionPoint, -Vector2.left, colliderRadius, _wallLayerMask);
        onWall = onLeftWall || onRightWall;

        // Ground callbacks
        if (!prevOnGround && onGround)
            OnGroundEnter();
        else if (prevOnGround && !onGround)
            OnGroundExit();

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
    
    void OnGroundEnter()
    {
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
        _animations.Squish(squishDuration, new Vector2((_animations.GetPlayerScale().x < 0 ? -1f : 1f) * squishXScale, squishYScale));

        // Landing Sound
        _playerAudio.PlayFootstepSFX();
    }

    void OnGroundExit()
    {
        // _animations.CreateDustTrail();
        _startedFalling = false;
        UpdateFallPosition();
    }

    void OnBigFall()
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawCube(_groundDetectionPoint, new Vector3(_coll.bounds.size.x * 0.95f, colliderRadius, 0f));
        Gizmos.DrawRay(_leftWallUpperDetectionPoint,  Vector2.left * colliderRadius);
        Gizmos.DrawRay(_leftWallLowerDetectionPoint,  -Vector2.left * colliderRadius);
        Gizmos.DrawRay(_rightWallUpperDetectionPoint,  Vector2.left * colliderRadius);
        Gizmos.DrawRay(_rightWallLowerDetectionPoint,  -Vector2.left * colliderRadius);
    }
}