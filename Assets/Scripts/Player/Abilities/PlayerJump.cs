using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using Zenject;

public class PlayerJump : MonoBehaviour
{
    [Header ("References")]
    private InputManager _inputManager;
    private Rigidbody2D _rb;
    private PlayerAbilityController _abilities;
    private PlayerPlatformCollision _coll;
    private PlayerAnimations _anim;
    private PlayerMovement _movement;

    [Header ("Jumping Settings")]
    [SerializeField] private float _jumpVelocity;
    [SerializeField] private float _fallMultiplier;
    [SerializeField] private float _lowJumpMultiplier;
    [SerializeField] private float _jumpBufferTime;
    [SerializeField] private float _coyoteTime;
    private bool _canJump = true;
    private float _jumpBuffer;
    private float _coyoteTimer;
    private bool _startsJumping;

    [Inject]
    public void Initialize(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _abilities = GetComponentInParent<PlayerAbilityController>();
        _coll = transform.parent.gameObject.GetComponent<PlayerPlatformCollision>();
        _anim = transform.parent.gameObject.GetComponent<PlayerAnimations>();
        _movement = transform.parent.gameObject.GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _inputManager.Event_GameplayInput_Jump += OnJump;
        _inputManager.Event_GameplayInput_JumpCanceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        _inputManager.Event_GameplayInput_Jump -= OnJump;
        _inputManager.Event_GameplayInput_JumpCanceled -= OnJumpCanceled;
    }

    public void EnablePlayerJump(bool enable, float time = 0f)
    {
        if (_canJump == enable) return;

        if (time == 0f)
            _canJump = enable;
        else
            Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _canJump = e, time, enable, !enable));
    }

    private void OnJump()
    {
        if (_canJump) {
            _jumpBuffer = _jumpBufferTime;
            _startsJumping = true;
        }
    }

    private void OnJumpCanceled()
    {
        _coyoteTimer = 0f;
    }

    private void Update()
    {
        if (!_canJump) return;

        // Coyote Time - allow late-input of jumps after touching the ground
        if (_coll.onGround) {
            _coyoteTimer = _coyoteTime;
        } else {
            _coyoteTimer -= Time.deltaTime;
        }

        // Jump Buffering - allow pre-input of jumps before touching the ground
        _jumpBuffer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (!_canJump) return;

        if (_startsJumping && _jumpBuffer > 0f && _coyoteTimer > 0f) {
            _startsJumping = false;
            _jumpBuffer = 0f;
            
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.velocity += Vector2.up * _jumpVelocity;

            if (_coll.onGround)
                _anim.CreateDustTrail();
        }

        // Low Jump
        if (_rb.velocity.y < 0) {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        } else if (_rb.velocity.y > 0 && !_inputManager.HasJumpInput()) {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
