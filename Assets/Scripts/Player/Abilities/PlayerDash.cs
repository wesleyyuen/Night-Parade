using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerDash : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerAnimations _anim;
    PlayerMovement _movement;
    PlayerAbilityController _abilities;
    PlayerPlatformCollision _collision;
    [HideInInspector] public bool isDashing;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float cooldown;
    [SerializeField] float freezeDuration;
    [SerializeField] ParticleSystem afterimage;
    ParticleSystemRenderer _particleRenderer;
    const int maxDash = 1;
    int _dashLeft;
    float _nextDashTime;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _movement = GetComponentInParent<PlayerMovement>();
        _abilities = GetComponentInParent<PlayerAbilityController>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
        _particleRenderer = afterimage.GetComponent<ParticleSystemRenderer>();

        _dashLeft = maxDash;
    }

    private void OnEnable()
    {
        InputManager.Event_Input_Dash += OnDash;
        _collision.Event_OnGroundEnter += ResetDash;
    }

    private void OnDisable()
    {
        InputManager.Event_Input_Dash -= OnDash;
        _collision.Event_OnGroundEnter -= ResetDash;
    }

    private void ResetDash()
    {
        _dashLeft = maxDash;
    }

    private void OnDash()
    {
        if (enabled && !PauseMenu.isPuased && Time.time > _nextDashTime) {
            if (_dashLeft > 0) {
                if (!_collision.onGround) _dashLeft--;
                _nextDashTime = Time.time + cooldown;
                Vector2 dir = _anim.IsFacingRight() ? -Vector2.left : Vector2.left;
                StartCoroutine(_Dash(dir));
            }
        }
    }

    IEnumerator _Dash(Vector2 dir)
    {
        // Pre-Dash Freeze Effect
        _anim.SetJumpFallAnimation();
        _movement.FreezePlayerPositionForSeconds(freezeDuration);
        yield return new WaitForSeconds(freezeDuration);

        ActuallyDash(dir);

        yield return new WaitForSeconds(dashTime);

        // Post-Dash Freeze Effect
        _movement.FreezePlayerPositionForSeconds(freezeDuration);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), false);
        yield return new WaitForSeconds(freezeDuration);
        _collision.UpdateFallPosition();
        _abilities.EnableAbility(PlayerAbilityController.Ability.Jump, true);
        _rb.drag = 1f;
        _rb.gravityScale = 1;
    }

    private void ActuallyDash(Vector2 dir)
    {
        isDashing = true;

        // Effects
        _particleRenderer.flip = new Vector3(dir.x > 0 ? 0f : 1f, 0f, 0f);
        afterimage.Play();

        // Movement
        _abilities.EnableAbility(PlayerAbilityController.Ability.Jump, false);
        _movement.LetRigidbodyMoveForSeconds(dashTime + freezeDuration);
        _rb.drag = dashSpeed * 0.1f;
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.velocity += dir.normalized * dashSpeed;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);
    }
}
