using System.Collections;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] ParticleSystem _dustTrail;
    PlayerPlatformCollision _grounded;
    Animator _playerAnimator;
    Animator _weaponAnimator;
    Rigidbody2D _rb;
    Collider2D _collider;
    PlayerMovement _movement;
    InputMaster _input;
    float _xRaw;
    bool _isSquishing;
    bool canTurn;

    void Awake()
    {
        _grounded = GetComponentInParent<PlayerPlatformCollision>();

        foreach (Transform obj in transform) {
            if (obj.tag == "Weapon")
                _weaponAnimator = obj.GetComponent<Animator>();
            else if (obj.tag == "Sprite")
                _playerAnimator = obj.GetComponent<Animator>();
        }
        
        _rb = GetComponentInParent<Rigidbody2D>();
        _collider = GetComponentInParent<Collider2D>();
        _movement = GetComponentInParent<PlayerMovement>();

        canTurn = true;
        FaceRight(_playerAnimator.transform.localScale.x == 1f);

        // Handle Input
        _input = new InputMaster();
        _input.Player.Movement.Enable();
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
        StartCoroutine(Utility.ChangeVariableAfterDelay<bool>(e => _weaponAnimator.enabled = e, time, false, true));
    }

    public void SetPlayerScale(Vector3 scale)
    {
        _playerAnimator.transform.localScale = scale;
        _weaponAnimator.transform.localScale = scale;
    }

    public Vector3 GetPlayerScale()
    {
        return _playerAnimator.transform.localScale;
    }

    void Update()
    {
        SetJumpFallAnimation();

        if (!canTurn) return;

        Vector3 prevLocalScale = _playerAnimator.transform.localScale;
        _xRaw = _input.Player.Movement.ReadValue<Vector2>().x;

        // Flip sprite
        if (_xRaw > 0 && prevLocalScale.x != 1f) {
            FaceRight(true); 
        } else if (_xRaw < 0 && prevLocalScale.x != -1f) {
            FaceRight(false);
        }
        if (!_isSquishing)
            FaceRight(_playerAnimator.transform.localScale.x == 1f);

        // Set animations
        if (_movement.canWalk) {
            SetRunAnimation(_xRaw);
        }
    }

    void SetBool(string name, bool val)
    {
        _playerAnimator.SetBool(name, val);
        _weaponAnimator.SetBool(name, val);
    }

    void SetFloat(string name, float val)
    {
        _playerAnimator.SetFloat(name, val);
        _weaponAnimator.SetFloat(name, val);
    }

    void SetInteger(string name, int val)
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

    public void CreateDustTrail()
    {
        _dustTrail.Play ();
    }

    public void Squish(float duration, Vector2 to)
    {
        StartCoroutine(SquishHelper(duration, to));
    }

    IEnumerator SquishHelper(float duration, Vector2 to)
    {
        _isSquishing = true;

        // Always return to scale of (-1/1, 1)
        Vector2 from = new Vector2(_playerAnimator.transform.localScale.x > 0 ? 1f : -1f, 1f);
        SetPlayerScale(from);
        for (float t = 0f, t2 = 0f; t < 1f; t += Time.deltaTime / (duration * 0.35f), t2 += Time.deltaTime / (duration * 0.35f)) {
            SetPlayerScale(new Vector3(Mathf.SmoothStep(from.x, to.x, t), Mathf.SmoothStep(from.y, to.y, t2), 1f));
            yield return null;
        }

        yield return new WaitForSeconds(duration * 0.45f);

        for (float t = 0f, t2 = 0f; t < 1f; t += Time.deltaTime / (duration * 0.2f), t2 += Time.deltaTime / (duration * 0.2f)) {
            SetPlayerScale(new Vector3(Mathf.SmoothStep(to.x, from.x, t), Mathf.SmoothStep(to.y, from.y, t2), 1f));
            yield return null;
        }

        SetPlayerScale(from);
        _isSquishing = false;
    }
}
