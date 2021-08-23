using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    Rigidbody2D _rb;
    PlayerAnimations _anim;
    PlayerPlatformCollision _collision;
    PlayerMovement _movement;
    PlayerAbilityController _abilities;
    [HideInInspector] public bool isDashing;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float cooldown;
    [SerializeField] float freezeDuration;
    [SerializeField] ParticleSystem afterimage;
    float _nextDashTime;
    bool _canDash;

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _movement = GetComponentInParent<PlayerMovement>();
        _abilities = GetComponentInParent<PlayerAbilityController>();
        Transform collTransform = transform.parent.Find("Platform Collision");
        _canDash = true;
        if (collTransform)
            _collision = collTransform.GetComponent<PlayerPlatformCollision>();
    }

    void Update()
    {
        if (!_canDash) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > _nextDashTime) {
            _nextDashTime = Time.time + cooldown;
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (dir == Vector2.zero)
                dir = -_rb.transform.localScale.x * Vector2.left;
            StartCoroutine(StartDash(dir));
        }
    }

    IEnumerator StartDash(Vector2 dir)
    {
        // Freeze position effect
        StartCoroutine( Utility.ChangeVariableAfterDelay<RigidbodyConstraints2D>(e => _rb.constraints = e, freezeDuration, RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation, RigidbodyConstraints2D.FreezeRotation));

        yield return new WaitForSeconds(freezeDuration);
        StartCoroutine(Dash(dir));
    }
    
    IEnumerator Dash (Vector2 dir)
    {
        isDashing = true;

        // Effects
        afterimage.GetComponent<ParticleSystemRenderer>().flip = new Vector3(dir.x > 0 ? 0f : 1f, 0f, 0f);
        afterimage.Play();

        _abilities.EnableAbility(PlayerAbilityController.Ability.Jump, false);
        _movement.EnablePlayerMovement(false);
        _rb.drag = dashSpeed * 0.1f;
        _rb.gravityScale = 0;

        _rb.velocity = Vector2.zero;
        _rb.velocity += dir.normalized * dashSpeed;

        yield return new WaitForSeconds(dashTime);

        DashEnd();
    }

    void DashEnd()
    {
        StartCoroutine( Utility.ChangeVariableAfterDelay<RigidbodyConstraints2D>(e => _rb.constraints = e, freezeDuration, RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation, RigidbodyConstraints2D.FreezeRotation));
        // TODO: lerp drag maybe
        _rb.drag = 0;
        _movement.EnablePlayerMovement(true);
        _abilities.EnableAbility(PlayerAbilityController.Ability.Jump, true);
        _rb.gravityScale = 1;
    }
}
