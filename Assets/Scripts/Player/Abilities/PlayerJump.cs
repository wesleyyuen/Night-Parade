using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header ("References")]
    Rigidbody2D _rb;
    [SerializeField] ParticleSystem dustTrail;
    PlayerPlatformCollision _coll;
    InputMaster _input;

    [Header ("Jumping Settings")]
    [SerializeField] float jumpVelocity;
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;
    [SerializeField] float jumpBufferTime;
    [SerializeField] float coyoteTime;
    bool _canJump = true;
    float _jumpBuffer;
    float _coyoteTimer;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _coll = gameObject.transform.parent.gameObject.GetComponentInChildren<PlayerPlatformCollision>();
        _canJump = true;

        // Handle Inputs
        _input = new InputMaster();
        _input.Player.Jump.Enable();
        _input.Player.Jump.started += OnJump;
    }


    public void EnablePlayerJump(bool enable, float time = 0f)
    {
        if (_canJump == enable) return;

        if (time == 0)
            _canJump = enable;
        else
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _canJump = e, time, enable, !enable));
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (_coll.onGround) {
            _jumpBuffer = jumpBufferTime;
        }
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
        } else if (_rb.velocity.y > 0 && _input.Player.Jump.ReadValue<float>() <= 0.5f) {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
