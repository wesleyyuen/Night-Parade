using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    [Header ("References")]
    Rigidbody2D _rb;
    PlayerPlatformCollision _coll;
    PlayerAnimations _anim;
    PlayerMovement _movement;
    InputMaster _input;

    [Header ("Jumping Settings")]
    [SerializeField] float _jumpVelocity;
    [SerializeField] float _fallMultiplier;
    [SerializeField] float _lowJumpMultiplier;
    [SerializeField] float _jumpBufferTime;
    [SerializeField] float _coyoteTime;
    [SerializeField] float _drag;
    bool _canJump = true;
    float _jumpBuffer;
    float _coyoteTimer;
    bool _startsJumping;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _coll = transform.parent.gameObject.GetComponent<PlayerPlatformCollision>();
        _anim = transform.parent.gameObject.GetComponent<PlayerAnimations>();
        _movement = transform.parent.gameObject.GetComponent<PlayerMovement>();
        _canJump = true;

        // Handle Inputs
        _input = new InputMaster();
        _input.Player.Jump.Enable();
        _input.Player.Jump.started += OnJump;
        _input.Player.Jump.canceled += OnJump;
    }


    public void EnablePlayerJump(bool enable, float time = 0f)
    {
        if (_canJump == enable) return;

        if (time == 0f)
            _canJump = enable;
        else
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _canJump = e, time, enable, !enable));
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) {
            _jumpBuffer = _jumpBufferTime;
            _startsJumping = true;
        } else if (context.canceled) {
            _coyoteTimer = 0f;
        }
    }

    void Update()
    {
        if (!_canJump) return;

        // Coyote Time - allow late-input of jumps after touching the ground
        if (_coll.onGround) {
            _rb.drag = 1f;
            _coyoteTimer = _coyoteTime;
        }
        else
            _coyoteTimer -= Time.deltaTime;

        // Jump Buffering - allow pre-input of jumps before touching the ground
        _jumpBuffer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (!_canJump) return;

        if (_startsJumping && _jumpBuffer > 0f && _coyoteTimer > 0f) {
            _startsJumping = false;
            _rb.drag = _drag;
            _jumpBuffer = 0f;
            
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.velocity += Vector2.up * _jumpVelocity;

            if (_coll.onGround)
                _anim.CreateDustTrail();
        }

        // Low Jump
        if (_rb.velocity.y < 0) {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        } else if (_rb.velocity.y > 0 && _input.Player.Jump.ReadValue<float>() <= 0.5f) {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
            _coyoteTimer = 0f;
        }
    }
}
