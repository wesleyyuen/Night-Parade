using System;
using UnityEngine;
using MEC;
using DG.Tweening;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _dustTrail;
    private PlayerPlatformCollision _grounded;
    private Animator _playerAnimator;
    private WeaponFSM _weaponFSM;
    private Animator _weaponAnimator;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private PlayerMovement _movement;
    private bool _isSquishing;
    private bool canTurn;

    private void Awake()
    {
        _grounded = GetComponentInParent<PlayerPlatformCollision>();

        foreach (Transform obj in transform) {
            if (obj.tag == "Weapon") {
                _weaponFSM = obj.GetComponent<WeaponFSM>();
                _weaponAnimator = obj.GetComponent<Animator>();
            }
            else if (obj.tag == "PlayerSprite")
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

    public void FreezePlayerAnimation(bool enable, float time)
    {
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _playerAnimator.enabled = e, time, enable, !enable).CancelWith(gameObject));
        Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<bool>(e => _weaponAnimator.enabled = e, time, enable, !enable).CancelWith(gameObject));
    }

    private void SetPlayerScale(Vector3 scale, float duration = 0f)
    {
        if (duration == 0f)
            _playerAnimator.transform.localScale = scale;
        else
            _playerAnimator.transform.DOScale(scale, duration);
        if (_weaponFSM.gameObject.TryGetComponent<WakizashiFSM>(out WakizashiFSM wakizashi)) {
            if (wakizashi.IsOnPlayer()) {
                if (duration == 0f)
                    _weaponAnimator.transform.localScale = scale;
                else
                    _weaponAnimator.transform.DOScale(scale, duration);
            }
        } else {
            if (duration == 0f)
                _weaponAnimator.transform.localScale = scale;
            else
                _weaponAnimator.transform.DOScale(scale, duration);
        }
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
        SetFloat("Vertical", Mathf.Approximately(_rb.velocity.y, 0) ? 0 : _rb.velocity.y);
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
        _dustTrail.Play();
    }

    public void Squish(float duration, Vector2 to)
    {
        _isSquishing = true;
        Vector2 from = new Vector2(_playerAnimator.transform.localScale.x > 0 ? 1f : -1f, 1f);
        float holdDuration = duration * 0.45f;
        SetPlayerScale(from);

        Sequence playerSequence = DOTween.Sequence();
        playerSequence.Append(_playerAnimator.transform.DOScale(to, duration * 0.35f).SetEase(Ease.OutElastic))
                      .Append(_playerAnimator.transform.DOScale(from, duration * 0.2f).SetEase(Ease.OutElastic)
                                                       .SetDelay(holdDuration)
                                                       .OnComplete(() => {
                                                            SetPlayerScale(from);
                                                            _isSquishing = false;
                                                       }));

        
        Sequence weaponSequence = DOTween.Sequence();
        if (_weaponFSM.gameObject.TryGetComponent<WakizashiFSM>(out WakizashiFSM wakizashi)) {
            if (wakizashi.IsOnPlayer()) {
                weaponSequence.Append(_weaponAnimator.transform.DOScale(to, duration * 0.35f).SetEase(Ease.OutElastic))
                              .Append(_weaponAnimator.transform.DOScale(from, duration * 0.2f).SetEase(Ease.OutElastic)
                                                               .SetDelay(holdDuration));
            }
        } else {
                weaponSequence.Append(_weaponAnimator.transform.DOScale(to, duration * 0.35f).SetEase(Ease.OutElastic))
                              .Append(_weaponAnimator.transform.DOScale(from, duration * 0.2f).SetEase(Ease.OutElastic)
                                                               .SetDelay(holdDuration));
        }
    }
}
