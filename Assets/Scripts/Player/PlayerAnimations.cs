using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] ParticleSystem dustTrail;
    [SerializeField] Animator swordAnimator;
    PlayerPlatformCollision _grounded;
    Animator _playerAnimator;
    Rigidbody2D _rb;
    PlayerMovement _movement;
    [HideInInspector] public bool canTurn;

    void Awake()
    {
        _grounded = GetComponent<PlayerPlatformCollision>();
        _playerAnimator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _movement = GetComponent<PlayerMovement>();
        canTurn = true;
    }

    public void EnablePlayerTurning(bool enable, float time = 0f)
    {
        if (time == 0)
            canTurn = enable;
        else
            StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => canTurn = e, time, enable, !enable));
    }

    public void FreezePlayerAnimation(float time)
    {
        StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _playerAnimator.enabled = e, time, false, true));
        StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => swordAnimator.enabled = e, time, false, true));
    }

    void Update()
    {
        SetJumpFallAnimation();

        if (!canTurn) return;

        float horizontalInput = Input.GetAxisRaw ("Horizontal");
        Vector3 prevLocalScale = transform.localScale;

        // Flip sprite (TODO: maybe move into child object)
        if (horizontalInput > 0 && prevLocalScale.x != 1f) {
            FaceRight(true);
        } else if (horizontalInput < 0 && prevLocalScale.x != -1f) {
            FaceRight(false);
        }

        // Set animations
        if (_movement.canWalk) {
            SetRunAnimation(horizontalInput);
        }
    }

    void SetBool(string name, bool val)
    {
        _playerAnimator.SetBool(name, val);
        swordAnimator.SetBool(name, val);
    }

    void SetFloat(string name, float val)
    {
        _playerAnimator.SetFloat(name, val);
        swordAnimator.SetFloat(name, val);
    }

    void SetInteger(string name, int val)
    {
        _playerAnimator.SetInteger(name, val);
        swordAnimator.SetInteger(name, val);
    }

    public void FaceRight(bool faceRight)
    {
        swordAnimator.SetBool ("FacingRight", faceRight);
        transform.localScale = new Vector3 (faceRight ? 1f : -1f, 1f, 1f);
    }

    public void SetRunAnimation(float horizontalInput)
    {
        SetFloat ("Horizontal", horizontalInput);
    }

    public void SetJumpFallAnimation()
    {
        SetBool ("IsGrounded", _grounded.onGround);
        SetFloat ("Vertical", _rb.velocity.y);
    }

    public void SetAttackAnimation(int count)
    {
        SetInteger("Attack", count);
    }

    public void SetBlockAnimation(bool val)
    {
        SetBool("IsBlocking", val);
    }

    public int GetCurrentAttackAnimation()
    {
        return _playerAnimator.GetInteger("Attack");
    }

    public void CreateDustTrail ()
    {
        if (_grounded.onGround) {
            dustTrail.Play ();
        }
    }
}
