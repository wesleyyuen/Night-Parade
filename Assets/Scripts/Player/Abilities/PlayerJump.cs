using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header ("References")]
    Rigidbody2D _rb;
    [SerializeField] ParticleSystem dustTrail;
    PlayerPlatformCollision _coll;

    [Header ("Jumping Settings")]
    [SerializeField] float jumpVelocity = 36f;
    [SerializeField] float fallMultiplier = 0.8f;
    [SerializeField] float lowJumpMultiplier = 5f;
    [SerializeField] float jumpBufferTime = 0.2f;
    [SerializeField] float coyoteTime = 0.05f;
    bool _canJump;
    float _jumpBuffer;
    float _coyoteTimer;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _coll = gameObject.transform.parent.gameObject.GetComponentInChildren<PlayerPlatformCollision>();
        _canJump = true;
    }

    public void EnablePlayerJump(bool enable, float time = 0f)
    {
        if (_canJump == enable) return;

        if (time == 0)
            _canJump = enable;
        else
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _canJump = e, time, enable, !enable));
    }

    void Update()
    {
        if (!_canJump) return;

        // Coyote Time - allow late-input of jumps after touching the ground
        _coyoteTimer -= Time.deltaTime;
        if (_coll.onGround) {
            _coyoteTimer = coyoteTime;
        }

        // Jump Buffering - allow pre-input of jumps before touching the ground
        _jumpBuffer -= Time.deltaTime;
        if (Input.GetButtonDown ("Jump") && _coll.onGround) {
            _jumpBuffer = jumpBufferTime;
        }
    }

    void FixedUpdate()
    {
        if (!_canJump) return;

        if (_jumpBuffer > 0 && _coyoteTimer > 0) {
            _jumpBuffer = 0;
            _coyoteTimer = 0;
            dustTrail.Play();
            
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.velocity += Vector2.up * jumpVelocity;
        }

        // Low Jump
        if (_rb.velocity.y < 0) {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (_rb.velocity.y > 0 && !Input.GetButton ("Jump")) {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
