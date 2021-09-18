using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerAnimations _anim;
    PlayerMovement _movement;
    PlayerAbilityController _abilities;
    InputMaster _input;
    [HideInInspector] public bool isDashing;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float cooldown;
    [SerializeField] float freezeDuration;
    [SerializeField] ParticleSystem afterimage;
    float _nextDashTime;
    bool _canDash = true;

    void Awake()
    {
        // Handle Input
        _input = new InputMaster();
        _input.Player.Dash.started += OnDash;
    }

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _movement = GetComponentInParent<PlayerMovement>();
        _abilities = GetComponentInParent<PlayerAbilityController>();
    }

    void OnEnable()
    {
        _input.Player.Movement.Enable();
        _input.Player.Dash.Enable();
    }

    void OnDisable()
    {
        _input.Player.Movement.Disable();
        _input.Player.Dash.Disable();
    }

    void OnDash(InputAction.CallbackContext context)
    {
        if (enabled && _canDash && Time.time > _nextDashTime) {
            _nextDashTime = Time.time + cooldown;
            Vector2 dir = _input.Player.Movement.ReadValue<Vector2>();
            if (dir == Vector2.zero)
                dir = -_rb.transform.localScale.x * Vector2.left;
            StartCoroutine(Dash(dir));
        }
    }

    IEnumerator Dash(Vector2 dir)
    {
        // Pre-Dash Freeze Effect
        _anim.SetJumpFallAnimation();
        _movement.FreezePlayerPosition(freezeDuration);
        yield return new WaitForSeconds(freezeDuration);

        // Actually Dash
        isDashing = true;

        // Effects
        afterimage.GetComponent<ParticleSystemRenderer>().flip = new Vector3(dir.x > 0 ? 0f : 1f, 0f, 0f);
        afterimage.Play();

        _abilities.EnableAbility(PlayerAbilityController.Ability.Jump, false);
        _movement.LetRigidbodyMoveForSeconds(dashTime + freezeDuration);
        _rb.drag = dashSpeed * 0.1f;
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.velocity += dir.normalized * dashSpeed;
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);

        yield return new WaitForSeconds(dashTime);

        // Post-Dash Freeze Effect
        _movement.FreezePlayerPosition(freezeDuration);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), false);
        yield return new WaitForSeconds(freezeDuration);
        _abilities.EnableAbility(PlayerAbilityController.Ability.Jump, true);
        _rb.drag = 1f;
        _rb.gravityScale = 1;
    }
}
