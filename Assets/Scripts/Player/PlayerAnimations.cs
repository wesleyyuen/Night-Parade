using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] ParticleSystem _dustTrail;
    PlayerPlatformCollision _grounded;
    Animator _playerAnimator;
    WeaponFSM _weaponFSM;
    Animator _weaponAnimator;
    Rigidbody2D _rb;
    Collider2D _collider;
    PlayerMovement _movement;
    bool _isSquishing;
    bool canTurn;

    private void Awake()
    {
        _grounded = GetComponentInParent<PlayerPlatformCollision>();

        foreach (Transform obj in transform) {
            if (obj.tag == "Weapon") {
                _weaponFSM = obj.GetComponent<WeaponFSM>();
                _weaponAnimator = obj.GetComponent<Animator>();
            }
            else if (obj.tag == "Sprite")
                _playerAnimator = obj.GetComponent<Animator>();
        }
        
        _rb = GetComponentInParent<Rigidbody2D>();
        _collider = GetComponentInParent<Collider2D>();
        _movement = GetComponentInParent<PlayerMovement>();

        canTurn = true;
    }

    private void Start()
    {
        FaceRight(_playerAnimator.transform.localScale.x == 1f);
    }

    public void EnablePlayerTurning(bool enable, float time = 0f)
    {
        if (time == 0)
            canTurn = enable;
        else
            Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => canTurn = e, time, enable, !enable).CancelWith(gameObject));
    }

    public void FreezePlayerAnimation(float time)
    {
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _playerAnimator.enabled = e, time, false, true).CancelWith(gameObject));
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _weaponAnimator.enabled = e, time, false, true).CancelWith(gameObject));
    }

    private void SetPlayerScale(Vector3 scale)
    {
        _playerAnimator.transform.localScale = scale;
        if (_weaponFSM.IsOnPlayer()) _weaponAnimator.transform.localScale = scale;
    }

    private void Update()
    {
        SetJumpFallAnimation();

        if (!canTurn) return;

        Vector3 prevLocalScale = _playerAnimator.transform.localScale;

        // Flip sprite
        if (InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Right) && prevLocalScale.x != 1f) {
            FaceRight(true); 
        } else if (InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Left) && prevLocalScale.x != -1f) {
            FaceRight(false);
        }
        if (!_isSquishing)
            FaceRight(_playerAnimator.transform.localScale.x == 1f);

        // Set animations
        if (_movement.canWalk) {
            float input = 0f;
            if (InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Right)) input = 1f;
            else if (InputManager.Instance.HasDirectionalInput(InputManager.DirectionInput.Left)) input = -1f;
            SetRunAnimation(input);
        }
    }

    private void SetTrigger(string name)
    {
        _playerAnimator.SetTrigger(name);
        _weaponAnimator.SetTrigger(name);
    }

    private void SetBool(string name, bool val)
    {
        _playerAnimator.SetBool(name, val);
        _weaponAnimator.SetBool(name, val);
    }

    private void SetFloat(string name, float val)
    {
        _playerAnimator.SetFloat(name, val);
        _weaponAnimator.SetFloat(name, val);
    }

    private void SetInteger(string name, int val)
    {
        _playerAnimator.SetInteger(name, val);
        _weaponAnimator.SetInteger(name, val);
    }

    public void FaceRight(bool faceRight)
    {
        _weaponAnimator.SetBool("FacingRight", faceRight);
        SetPlayerScale(new Vector3 (faceRight ? 1f : -1f, _playerAnimator.transform.localScale.y, 1f));
        _collider.offset = new Vector2(faceRight ? 0.12f : -0.12f, 1.5f);
    }

    public bool IsFacingRight()
    {
        return _playerAnimator.transform.localScale.x > 0f;
    }

    public void SetRunAnimation(float horizontalInput)
    {
        SetFloat("Horizontal", horizontalInput);
    }

    public void SetJumpFallAnimation()
    {
        SetBool("IsGrounded", _grounded.onGround);
        SetFloat("Vertical", _rb.velocity.y);
    }

    public void SetAttackAnimation(int count)
    {
        SetInteger("DirectionalInput", (int) InputManager.Instance.GetDirectionalInput());
        SetInteger("AttackCount", count);
        if (count != 0) {
            SetTrigger("Attack");
        }
    }

    public void SetBlockAnimation(bool val)
    {
        SetBool("IsBlocking", val);
    }

    public int GetCurrentAttackAnimation()
    {
        return _playerAnimator.GetInteger("AttackCount");
    }

    public void SetThrowAnimation()
    {
        _weaponAnimator.SetBool("IsThrown", true);
    }

    public void SetUnlodgedAnimation()
    {
        _weaponAnimator.SetBool("IsThrown", false);
    }

    public void CreateDustTrail()
    {
        _dustTrail.Play ();
    }

    public void Squish(float duration, Vector2 to)
    {
        Timing.RunCoroutine(_SquishCoroutine(duration, to).CancelWith(gameObject));
    }

    private IEnumerator<float> _SquishCoroutine(float duration, Vector2 to)
    {
        _isSquishing = true;

        // Always return to scale of (-1/1, 1)
        Vector2 from = new Vector2(_playerAnimator.transform.localScale.x > 0 ? 1f : -1f, 1f);
        SetPlayerScale(from);
        for (float t = 0f, t2 = 0f; t < 1f; t += Timing.DeltaTime / (duration * 0.35f), t2 += Timing.DeltaTime / (duration * 0.35f)) {
            SetPlayerScale(new Vector3(Mathf.SmoothStep(from.x, to.x, t), Mathf.SmoothStep(from.y, to.y, t2), 1f));
            yield return Timing.WaitForOneFrame;
        }

        yield return Timing.WaitForSeconds(duration * 0.45f);

        for (float t = 0f, t2 = 0f; t < 1f; t += Timing.DeltaTime / (duration * 0.2f), t2 += Timing.DeltaTime / (duration * 0.2f)) {
            SetPlayerScale(new Vector3(Mathf.SmoothStep(to.x, from.x, t), Mathf.SmoothStep(to.y, from.y, t2), 1f));
            yield return Timing.WaitForOneFrame;
        }

        SetPlayerScale(from);
        _isSquishing = false;
    }
}
