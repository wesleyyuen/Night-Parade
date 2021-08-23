using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header ("References")]
    PlayerPlatformCollision _coll;
    Rigidbody2D _rb;
    PlayerAnimations _animations;
    PlayerAudio _audio;

    [Header ("Movement Settings")]
    [SerializeField] float _movementSpeed = 14f;
    public bool canWalk {get; private set;}
    public bool isHandicapped {get; set;}
    float _xRaw, _yRaw;
    bool _isLetRBMove;

    void Awake ()
    {
        _coll = GetComponentInChildren<PlayerPlatformCollision>();
        _rb = GetComponent<Rigidbody2D>();
        _animations = GetComponent<PlayerAnimations>();
        _audio = GetComponent<PlayerAudio>();
        canWalk = true;
        _isLetRBMove = false;
    }

    public void EnablePlayerMovement(bool enable, float time = 0)
    {
        if (time == 0)
            canWalk = enable;
        else
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => canWalk = e, time, enable, !enable));
    }

    public void FreezePlayerPosition(float time)
    {
        StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _isLetRBMove = e, time, true, false));
        StartCoroutine(Utility.ChangeVariableAfterDelay<Vector2>(e => _rb.velocity = e, time, Vector2.zero, _rb.velocity));
    }

    void Update()
    {
        _xRaw = Input.GetAxisRaw("Horizontal");
        _yRaw = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate ()
    {
        if (_isLetRBMove) {
            return;
        } else if (!canWalk && _coll.onGround) {
            _rb.velocity = new Vector2 (0f, _rb.velocity.y);
            return;
        }

        // Move player
        Vector2 newVelocity = _rb.velocity;
        if (_coll.onGround && _coll.onSlope) {
            newVelocity = new Vector2 (-_xRaw * _movementSpeed * _coll.slopeVector.x, -_xRaw * _movementSpeed * _coll.slopeVector.y);
        } else {
            newVelocity = new Vector2 (_xRaw * _movementSpeed, _rb.velocity.y);
        }

        if (isHandicapped)
            _rb.velocity = Vector2.Lerp(_rb.velocity, newVelocity, Time.deltaTime * 0.1f);
        else 
            _rb.velocity = newVelocity;
    }

    public IEnumerator HandicapMovement(float time)
    {
        isHandicapped = true;

        yield return new WaitForSeconds(time);

        isHandicapped = false;
    }

    public IEnumerator MoveForwardForSeconds(float time)
    {
        float timer = 0f;

        _isLetRBMove = true;
        _animations.EnablePlayerTurning(false);
        bool facingRight = transform.localScale.x > 0f;

        while (timer < time) {
            timer += Time.deltaTime;
            _animations.SetRunAnimation(transform.localScale.x);
            _rb.velocity = new Vector2((facingRight ? 1f : -1f) *_movementSpeed, 0f);
            yield return null;
        }
        
        _animations.EnablePlayerTurning(true);
        _isLetRBMove = false;
        _rb.velocity = new Vector2 (0f, _rb.velocity.y);
    }

    public void StepForward(float dist)
    {
        if (_coll.onGround && _xRaw != 0) {
            bool facingRight = transform.localScale.x > 0;
            _rb.velocity = Vector2.zero;
            _rb.MovePosition(_rb.position + new Vector2((facingRight ? 1f : -1f) * dist, 0f));
            _audio.PlayFootstepSFX();
        }
    }

    public void ApplyKnockback(Vector2 dir, float force, float time)
    {
        
        _animations.EnablePlayerTurning(false, time);
        EnablePlayerMovement(false, time);
        StartCoroutine(Utility.ChangeVariableAfterDelay<float>(e => _rb.drag = e, time == 0 ? 0.1f : time, 3f, 0));
        StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _isLetRBMove = e, time == 0 ? 0.1f : time, true, false));

        _rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    }
}