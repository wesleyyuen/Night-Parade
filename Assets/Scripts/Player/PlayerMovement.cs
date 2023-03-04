using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity​Engine.Experimental.Rendering.Universal;
using MEC;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [Header ("References")]
    private InputManager _inputManager;
    private PlayerPlatformCollision _coll;
    private Rigidbody2D _rb;
    private PlayerAnimations _animations;
    private PlayerAudio _audio;
    private PixelPerfectCamera _ppc;

    [Header ("Movement Settings")]
    [SerializeField] private float _movementSpeed = 14f;
    public bool canWalk {get; private set;}
    public bool isHandicapped {get; set;}
    private bool _isLetRBMove;
    private float _originalMovementSpeed;

    [Inject]
    public void Initialize(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        _coll = GetComponent<PlayerPlatformCollision>();
        _rb = GetComponent<Rigidbody2D>();
        _animations = GetComponent<PlayerAnimations>();
        _audio = GetComponentInChildren<PlayerAudio>();
        _ppc = Camera.main.GetComponent<PixelPerfectCamera>();

        canWalk = true;
        _isLetRBMove = false;
        _originalMovementSpeed = _movementSpeed;
    }

    public void EnablePlayerMovement(bool enable, float time = 0)
    {
        if (time == 0)
            canWalk = enable;
        else
            Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => canWalk = e, time, enable, !enable).CancelWith(gameObject));
    }

    public void ChangePlayerMovementSpeed(bool changing, float speedMultiplier, float time)
    {
        if (changing) {
            if (time == 0)
                _movementSpeed = _originalMovementSpeed * speedMultiplier;
            else
                Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<float>(e => _movementSpeed = e, time, _originalMovementSpeed * speedMultiplier, _originalMovementSpeed).CancelWith(gameObject));
        } else {
            _movementSpeed = _originalMovementSpeed;
        }
    }

    public void FreezePlayerPosition(bool shouldFreeze, float time = 0f)
    {
        Vector2 orignal = _rb.velocity;
        if (time == 0f) {
            _isLetRBMove = shouldFreeze;
            _rb.isKinematic = shouldFreeze;
            _rb.velocity = shouldFreeze ? Vector2.zero : orignal;
        } else {
            LetRigidbodyMoveForSeconds(time);
            Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _rb.isKinematic = e, time, true, false).CancelWith(gameObject));
            Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<Vector2>(e => _rb.velocity = e, time, Vector2.zero, orignal).CancelWith(gameObject));
        }
    }

    public void LetRigidbodyMoveForSeconds(float time)
    {
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _isLetRBMove = e, time, true, false).CancelWith(gameObject));
    }

    private void FixedUpdate()
    {
        if (!_isLetRBMove && !canWalk && _coll.onGround) {
            _animations.SetRunAnimation(0f);
            _rb.velocity = new Vector2 (0f, _rb.velocity.y);
            return;
        } else if (_isLetRBMove) {
            return;
        }

        float horizotalInput = _inputManager.GetDirectionalInputVector().x;
        Vector2 newVelocity = _rb.velocity;
        
        // Move player
        // TODO: up slope still seems a bit slower then normal movement/upslope
        // if (_coll.onGround && _coll.onSlope && !_inputManager.HasJumpInput()) {
        //     newVelocity = new Vector2(horizotalInput * _movementSpeed, _rb.velocity.y);
        //     newVelocity = Vector3.ProjectOnPlane(newVelocity, _coll.slopeNormal);
        // }
        // else
            newVelocity = new Vector2(horizotalInput * _movementSpeed, _rb.velocity.y);

        if (isHandicapped)
            _rb.velocity = Vector2.Lerp(_rb.velocity, newVelocity, Time.deltaTime * 0.1f);
        else 
            _rb.velocity = newVelocity;
    }

    public void HandicapMovementForSeconds(float time)
    {
        Timing.RunCoroutine(_HandicapMovementCoroutine(time).CancelWith(gameObject));
    }

    private IEnumerator<float> _HandicapMovementCoroutine(float time)
    {
        isHandicapped = true;

        yield return Timing.WaitForSeconds(time);

        isHandicapped = false;
    }

    public void MoveForwardForSeconds(float time)
    {
        Timing.RunCoroutine(_MoveForwardForSecondsCoroutine(time, _animations.IsFacingRight()).CancelWith(gameObject));
    }

    private IEnumerator<float> _MoveForwardForSecondsCoroutine(float time, bool toRight)
    {
        float timer = 0f;

        _isLetRBMove = true;
        _animations.EnablePlayerTurning(false);

        while (timer < time) {
            timer += Time.deltaTime;
            _animations.SetRunAnimation(toRight ? 1f : -1f);
            _rb.velocity = (_animations.IsFacingRight() ? Vector2.right : Vector2.left) * _movementSpeed;
            yield return Timing.WaitForOneFrame;
        }
        
        _animations.EnablePlayerTurning(true);
        _isLetRBMove = false;
        _rb.velocity = new Vector2 (0f, _rb.velocity.y);
    }

    public void StepForward(float dist)
    {
        if (Constant.STOP_WHEN_ATTACK && _coll.onGround && _inputManager.GetDirectionalInputVector().x != 0)
            Timing.RunCoroutine(_StepForwardCoroutine(_movementSpeed * 3f, 0.035f).CancelWith(gameObject));
    }

    private IEnumerator<float> _StepForwardCoroutine(float speed, float time)
    {
        float timer = 0f;

        _isLetRBMove = true;
        _animations.EnablePlayerTurning(false);
        _rb.velocity = Vector2.zero;

        while (timer < time) {
            timer += Timing.DeltaTime;
            _rb.velocity = (_animations.IsFacingRight() ? Vector2.right : Vector2.left) * speed;
            yield return Timing.WaitForOneFrame;
        }

        _animations.EnablePlayerTurning(true);
        _isLetRBMove = false;

        _audio.PlayFootstepSFX();
    }

    public void ApplyKnockback(Vector2 dir, float force, float time)
    {
        _animations.EnablePlayerTurning(false, time);
        LetRigidbodyMoveForSeconds(time == 0 ? 0.1f : time);
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<float>(e => _rb.drag = e, time == 0 ? 0.1f : time, force * 0.1f, 1f).CancelWith(gameObject));
        
        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    }
}