using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerAnimations _anim;
    private PlayerMovement _movement;
    private PlayerHealth _health;
    private PlayerAbilityController _abilities;
    private PlayerPlatformCollision _collision;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float cooldown;
    [SerializeField] private float freezeDuration;
    [SerializeField] private float _counterWindow;
    // [SerializeField] private ParticleSystem afterimage;
    private HashSet<int> _enemiesParriedIDs = new HashSet<int>();
    // private ParticleSystemRenderer _particleRenderer;
    private Vector2 _blockPoint = new Vector2(0.95f, 1.45f);
    private Vector2 _blockRange = new Vector2(0.4f, 2.85f);
    private const int MAX_DASH = 1;
    private int _dashLeft;
    private float _nextDashTime;
    private float _counterTimer;

    private void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _anim = GetComponentInParent<PlayerAnimations>();
        _movement = GetComponentInParent<PlayerMovement>();
        _health = GetComponentInParent<PlayerHealth>();
        _abilities = GetComponentInParent<PlayerAbilityController>();
        _collision = GetComponentInParent<PlayerPlatformCollision>();
        // _particleRenderer = afterimage.GetComponent<ParticleSystemRenderer>();

        _dashLeft = MAX_DASH;
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
        _dashLeft = MAX_DASH;
    }

    private void OnDash()
    {
        if (enabled && !PauseMenu.isPuased && Time.time > _nextDashTime && _dashLeft > 0) {
            if (!_collision.onGround) {
                _dashLeft--;
            }
            _enemiesParriedIDs.Clear();
            _counterTimer = 0f;
            _nextDashTime = Time.time + cooldown;
            Vector2 dir = _anim.IsFacingRight() ? -Vector2.left : Vector2.left;
            StartCoroutine(_Dash(dir));
        }
    }

    public void Update()
    {
        _counterTimer += Time.deltaTime;
        
        if (_counterTimer <= _counterWindow) {
            Counter();
        }
    }

    private IEnumerator _Dash(Vector2 dir)
    {
        _health.isInvulnerable = true;
        _abilities.EnableAbility(PlayerAbilityController.Ability.Jump, false);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);

        // Pre-Dash Freeze Effect
        _anim.SetJumpFallAnimation();
        _movement.FreezePlayerPositionForSeconds(freezeDuration);
        yield return new WaitForSeconds(freezeDuration);

        ActuallyDash(dir);
        yield return new WaitForSeconds(dashTime);

        // Post-Dash Freeze Effect
        _movement.FreezePlayerPositionForSeconds(freezeDuration / 2);
        yield return new WaitForSeconds(freezeDuration / 2);

        // Reset
        _collision.UpdateFallPosition();

        _rb.drag = 1f;
        _rb.gravityScale = 1f;

        _health.isInvulnerable = false;
        _abilities.EnableAbility(PlayerAbilityController.Ability.Jump, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), false);
    }

    private void ActuallyDash(Vector2 dir)
    {
        // After Image Effects
        // _particleRenderer.flip = new Vector3(dir.x > 0 ? 0f : 1f, 0f, 0f);
        // afterimage.Play();

        // Movement
        _movement.LetRigidbodyMoveForSeconds(dashTime + freezeDuration / 2);
        _movement.LetRigidbodyMoveForSeconds(dashTime);
        _rb.gravityScale = 0f;
        _rb.drag = dashSpeed * 0.1f;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.velocity += dir.normalized * dashSpeed;
    }

    private void Counter()
    {
        Vector2 origin = (Vector2) _rb.transform.TransformPoint(new Vector3((_anim.IsFacingRight() ? 1f : -1f) * _blockPoint.x, _blockPoint.y));
        Collider2D[] parried = Physics2D.OverlapAreaAll(origin + new Vector2(-_blockRange.x/2, _blockRange.y/2),
                                                        origin + new Vector2(_blockRange.x/2, -_blockRange.y/2),
                                                        LayerMask.GetMask("Enemies"));
        if (parried.Length == 0) return;

        foreach (Collider2D hit in parried) {
            // Parry enemy only ONCE by adding them into list
            if (_enemiesParriedIDs.Add (hit.gameObject.GetInstanceID ())) {
                if (hit.TryGetComponent<EnemyFSM>(out EnemyFSM enemy) && !enemy.IsDead() && enemy.IsAttacking()) {
                    Vector2 hitDir = _anim.IsFacingRight() ? Vector2.right : Vector2.left;
                    enemy.TakeDamage(3f, hitDir);
                }
            }
        }

        TimeManager.Instance.SlowTimeForSeconds(0.3f, 0.5f);
    }
}
