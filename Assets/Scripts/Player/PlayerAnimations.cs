using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] ParticleSystem _dustTrail;
    PlayerPlatformCollision _grounded;
    Animator _playerAnimator;
    Animator _weaponAnimator;
    Rigidbody2D _rb;
    PlayerMovement _movement;
    InputMaster _input;
    float _xRaw;
    [HideInInspector] public bool canTurn;

    void Awake()
    {
        _grounded = GetComponent<PlayerPlatformCollision>();
        _playerAnimator = GetComponent<Animator>();

        foreach (Transform obj in transform) {
            if (obj.tag == "Weapon")
                _weaponAnimator = obj.GetComponent<Animator>();
        }
        
        _rb = GetComponent<Rigidbody2D>();
        _movement = GetComponent<PlayerMovement>();
        canTurn = true;

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

    void Update()
    {
        ReattachAnimatorReference();
        SetJumpFallAnimation();

        if (!canTurn) return;

        Vector3 prevLocalScale = transform.localScale;
        _xRaw = _input.Player.Movement.ReadValue<Vector2>().x;

        // Flip sprite (TODO: maybe move into child object)
        if (_xRaw > 0 && prevLocalScale.x != 1f) {
            FaceRight(true);
            Utility.UnflipGameObjectRecursively(gameObject, true, true);
        } else if (_xRaw < 0 && prevLocalScale.x != -1f) {
            FaceRight(false);
            Utility.UnflipGameObjectRecursively(gameObject, false, true);
        }

        // Set animations
        if (_movement.canWalk) {
            SetRunAnimation(_xRaw);
        }
    }

    void ReattachAnimatorReference()
    {
        if (_playerAnimator == null)
            _playerAnimator = GetComponent<Animator>();

        if (_weaponAnimator == null) {
            foreach (Transform obj in transform) {
                if (obj.tag == "Weapon")
                    _weaponAnimator = obj.GetComponent<Animator>();
            }
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
        _weaponAnimator.SetBool ("FacingRight", faceRight);
        transform.localScale = new Vector3 (faceRight ? 1f : -1f, 1f, 1f);
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

    public void CreateDustTrail ()
    {
        if (_grounded.onGround) {
            _dustTrail.Play ();
        }
    }
}
