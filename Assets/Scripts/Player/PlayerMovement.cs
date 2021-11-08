using System.Collections;
using UnityEngine;
using Unity​Engine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header ("References")]
    PlayerPlatformCollision _coll;
    Rigidbody2D _rb;
    PlayerAnimations _animations;
    PlayerAudio _audio;
    InputMaster _input;
    PixelPerfectCamera _ppc;
    [SerializeField] PhysicsMaterial2D _oneFriction;
    PhysicsMaterial2D _originalMaterial;

    [Header ("Movement Settings")]
    [SerializeField] float _movementSpeed = 14f;
    public bool canWalk {get; private set;}
    public bool isHandicapped {get; set;}
    float _xRaw, _yRaw;
    bool _isLetRBMove;
    float _originalMovementSpeed;

    void Awake()
    {
        _coll = GetComponent<PlayerPlatformCollision>();
        _rb = GetComponent<Rigidbody2D>();
        _animations = GetComponent<PlayerAnimations>();
        _audio = GetComponentInChildren<PlayerAudio>();
        _ppc = Camera.main.GetComponent<PixelPerfectCamera>();
        _originalMaterial = _rb.sharedMaterial;

        canWalk = true;
        _isLetRBMove = false;
        _originalMovementSpeed = _movementSpeed;

        // Handle Inputs
        _input = new InputMaster();
        _input.Player.Movement.Enable();
        _input.Player.Jump.Enable();
    }

    public void EnablePlayerMovement(bool enable, float time = 0)
    {
        if (time == 0)
            canWalk = enable;
        else
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => canWalk = e, time, enable, !enable));
    }

    public void ChangePlayerMovementSpeed(bool changing, float speedMultiplier, float time)
    {
        if (changing) {
            if (time == 0)
                _movementSpeed = _originalMovementSpeed * speedMultiplier;
            else
                StartCoroutine(Utility.ChangeVariableAfterDelay<float>(e => _movementSpeed = e, time, _originalMovementSpeed * speedMultiplier, _originalMovementSpeed));
        } else {
            _movementSpeed = _originalMovementSpeed;
        }
    }

    public void FreezePlayerPosition(float time)
    {
        LetRigidbodyMoveForSeconds(time);
        StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _rb.isKinematic = e, time, true, false));
        StartCoroutine(Utility.ChangeVariableAfterDelay<Vector2>(e => _rb.velocity = e, time, Vector2.zero, _rb.velocity));
    }

    public void LetRigidbodyMoveForSeconds(float time)
    {
        StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _isLetRBMove = e, time, true, false));
    }

    void Update()
    {
        Vector2 inputVector = _input.Player.Movement.ReadValue<Vector2>();
        _xRaw = inputVector.x;
        _yRaw = inputVector.y;
        
        // Disable pixel snapping when player is moving to avoid animation jittering
        // _ppc.pixelSnapping = _rb.velocity == Vector2.zero;
    }

    void FixedUpdate()
    {
        if (!_isLetRBMove && !canWalk && _coll.onGround) {
            _rb.velocity = new Vector2 (0f, _rb.velocity.y);
            return;
        } else if (_isLetRBMove) {
            return;
        }

        // Handle slope sliding
        _rb.sharedMaterial = _coll.onSlope && _xRaw == 0f ? _oneFriction : _originalMaterial;

        // Move player
        Vector2 newVelocity = _rb.velocity;
        
        // TODO: avoid reading jump input from here
        // TODO: up slope still seems a bit slower then normal movement/upslope
        bool notJumping = _input.Player.Jump.ReadValue<float>() <= 0.5f;
        if (_coll.onGround && _coll.onSlope && notJumping)
            newVelocity = new Vector2(_xRaw * _movementSpeed * Mathf.Abs(_coll.slopeVector.x), _xRaw * _movementSpeed * _coll.slopeVector.y);
        else
            newVelocity = new Vector2(_xRaw * _movementSpeed, _rb.velocity.y);

        if (isHandicapped)
            _rb.velocity = Vector2.Lerp(_rb.velocity, newVelocity, Time.deltaTime * 0.1f);
        else 
            _rb.velocity = newVelocity;
    }

    public void HandicapMovementForSeconds(float time)
    {
        StartCoroutine(HandicapMovementCoroutine(time));
    }

    IEnumerator HandicapMovementCoroutine(float time)
    {
        isHandicapped = true;

        yield return new WaitForSeconds(time);

        isHandicapped = false;
    }

    public void MoveForwardForSeconds(float time)
    {
        StartCoroutine(MoveForwardForSecondsCoroutine(time));
    }

    IEnumerator MoveForwardForSecondsCoroutine(float time)
    {
        float timer = 0f;

        _isLetRBMove = true;
        _animations.EnablePlayerTurning(false);
        Vector3 playerScale = _animations.GetPlayerScale();
        bool facingRight = playerScale.x > 0f;

        while (timer < time) {
            timer += Time.deltaTime;
            _animations.SetRunAnimation(playerScale.x);
            _rb.velocity = _coll.slopeVector * _movementSpeed;
            yield return null;
        }
        
        _animations.EnablePlayerTurning(true);
        _isLetRBMove = false;
        _rb.velocity = new Vector2 (0f, _rb.velocity.y);
    }

    public void StepForward(float dist)
    {
        if (Constant.stopWhenAttack && _coll.onGround && _xRaw != 0)
            StartCoroutine(StepForwardCoroutine(_movementSpeed * 3f, 0.035f));
    }

    IEnumerator StepForwardCoroutine(float speed, float time)
    {
        float timer = 0f;

        _isLetRBMove = true;
        _animations.EnablePlayerTurning(false);
        bool facingRight = _animations.GetPlayerScale().x > 0;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        while (timer < time) {
            timer += Time.deltaTime;
            _rb.velocity = _coll.slopeVector * speed;
            yield return null;
        }

        _animations.EnablePlayerTurning(true);
        _isLetRBMove = false;

        _audio.PlayFootstepSFX();
    }

    public void ApplyKnockback(Vector2 dir, float force, float time)
    {
        _animations.EnablePlayerTurning(false, time);
        LetRigidbodyMoveForSeconds(time == 0 ? 0.1f : time);
        StartCoroutine(Utility.ChangeVariableAfterDelay<float>(e => _rb.drag = e, time == 0 ? 0.1f : time, force * 0.1f, 1f));
        
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    }
}