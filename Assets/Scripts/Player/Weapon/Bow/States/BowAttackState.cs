using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class BowAttackState : IWeaponState, IBindInput
{
    private BowFSM _fsm;
    private PlayerMovement _playerMovement;
    private PlayerAnimations _playerAnimation;
    private PlayerAbilityController _abilityController;
    private const int _kMaxAirAttack = 2; // Light attack as 1; Heavy attack as 2
    private const int _kMaxComboCount = 2;
    private int _currentAttackCount;
    private bool _isListeningForNextAction;
    private bool _hasNextAttack;
    private bool _hasNextBlock;
    private bool _hasNextShoot;
    private bool _playedMissSFX;
    private HashSet<int> _enemiesAttackedIDs;

    public BowAttackState(BowFSM fsm)
    {
        _fsm = fsm;
        _playerMovement = fsm.GetComponentInParent<PlayerMovement>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void BindInput()
    {
        _fsm.InputActions.Player.Attack.started += OnNextAttack;
        // _fsm.InputActions.Player.Attack.performed += OnNextAttack;
        _fsm.InputActions.Player.Block.started += OnNextBlock;
        _fsm.InputActions.Player.Shoot_Hold.performed += OnNextShot;
    }

    public void UnbindInput()
    {
        _fsm.InputActions.Player.Attack.started -= OnNextAttack;
        // _fsm.InputActions.Player.Attack.performed -= OnNextAttack;
        _fsm.InputActions.Player.Block.started -= OnNextBlock;
        _fsm.InputActions.Player.Shoot_Hold.performed -= OnNextShot;
    }

    public void EnterState()
    {
        if (Constant.STOP_WHEN_ATTACK)
            _playerMovement.EnablePlayerMovement(false);

        _fsm.attackCooldownTimer = _fsm.weaponData.attackCooldown;
        _enemiesAttackedIDs = new HashSet<int> ();
        _isListeningForNextAction = false;
        _hasNextAttack = false;
        _hasNextBlock = false;
        _playedMissSFX = false;

        // Begin First Attack
        _currentAttackCount = 1;
        _playerAnimation.SetAttackAnimation(_currentAttackCount);
        _playerMovement.StepForward(2f);
    }

    private void OnNextAttack(InputAction.CallbackContext context)
    {
        // Listen for attack and queue as next action
        if (context.started && _currentAttackCount > 0) {
            if (_isListeningForNextAction) {
                _hasNextAttack = true;
                _hasNextBlock = false;

                _isListeningForNextAction = false;
            }
        }
        // else if (context.performed) {
        //     OnChargeAttack();
        // }
    }

    private void OnNextBlock(InputAction.CallbackContext context)
    {
        // Listen for block and queue as next action
        if (context.started && _currentAttackCount > 0) {
            if (_isListeningForNextAction) {
                _hasNextBlock = true;
                _isListeningForNextAction = false;
            }
        }
        // else if (context.performed) {
        //     OnChargeAttack();
        // }
    }

    // private void OnChargeAttack()
    // {
    // }

    private void OnNextShot(InputAction.CallbackContext context)
    {
        // Listen for throw and queue as next action
        if (context.started && _currentAttackCount > 0) {
            if (_isListeningForNextAction) {
                _hasNextShoot = true;
                _isListeningForNextAction = false;
            }
        }
    }

    public void Update()
    {
        // Needed to constantly turn it off to avoid player moving after taking dmg
        if (Constant.STOP_WHEN_ATTACK) _playerMovement.EnablePlayerMovement(false);

        if (_hasNextAttack && _fsm.attackCooldownTimer <= 0) {
            _fsm.attackCooldownTimer = _fsm.weaponData.attackCooldown;
            _currentAttackCount = (_currentAttackCount % _kMaxComboCount) + 1;
            _playerAnimation.SetAttackAnimation(_currentAttackCount);
            _playerMovement.StepForward(2f);
            _hasNextAttack = false;
            _playedMissSFX = false;
        }
    }

    public void FixedUpdate()
    {
    }

    public void ExitState()
    {
        _playerAnimation.EnablePlayerTurning(true);
        _playerAnimation.SetAttackAnimation(0);
        _playerMovement.EnablePlayerMovement(true);
        _enemiesAttackedIDs.Clear();
    }

    public void Attack(bool isListeningForNextAttack)
    {
        _isListeningForNextAction = isListeningForNextAttack;

        // Get Colliders of enemies hit
        Vector2 attackPoint = new Vector2((_playerAnimation.IsFacingRight() ? 1f : -1f) * _fsm.weaponData.attackPoint.x, _fsm.weaponData.attackPoint.y);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (_fsm.player.transform.TransformPoint(attackPoint), _fsm.weaponData.attackRange, 360, _fsm.weaponData.enemyLayers);

        // No Hits
        if (hitEnemies.Length == 0) {
            if (!_playedMissSFX && _enemiesAttackedIDs.Count == 0) {
                _fsm.PlayWeaponMissSFX();
                _playedMissSFX = true;
            }
            return;
        }

        Vector2 hitDir = _playerAnimation.IsFacingRight() ? Vector2.right : Vector2.left;
        if (DealDamage(hitEnemies, hitDir)) {
            // Utility.FreezePlayer(0.05f);
            _playerMovement.ApplyKnockback(-hitDir, _fsm.weaponData.horizontalKnockBackForce, 0.05f);
            CameraShake.Instance.ShakeCamera(1f, 0.1f);
            _fsm.PlayWeaponHitSFX();
        }
    }

    public void Upthrust(bool isListeningForNextAttack)
    {
        _isListeningForNextAction = isListeningForNextAttack;

        // Get Colliders of enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(_fsm.player.transform.TransformPoint(_fsm.weaponData.upthrustPoint), _fsm.weaponData.upthrustRange, 360, _fsm.weaponData.enemyLayers);

        // No Hits
        if (hitEnemies.Length == 0) {
            if (!_playedMissSFX && _enemiesAttackedIDs.Count == 0) {
                _fsm.PlayWeaponMissSFX();
                _playedMissSFX = true;
            }
            return;
        }

        if (DealDamage(hitEnemies, Vector2.up)) {
            // Utility.FreezePlayer(0.05f);
            _playerMovement.ApplyKnockback(Vector2.down, _fsm.weaponData.upthrustKnockBackForce, 0.05f);
            CameraShake.Instance.ShakeCamera(1f, 0.1f);
            _fsm.PlayWeaponHitSFX();
        }
    }

    public void Downthrust(bool isListeningForNextAttack)
    {
        _isListeningForNextAction = isListeningForNextAttack;

        // Get Colliders of enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(_fsm.player.transform.TransformPoint(_fsm.weaponData.downthrustPoint), _fsm.weaponData.downthrustRange, 360, _fsm.weaponData.enemyLayers);

        // No Hits
        if (hitEnemies.Length == 0) {
            if (!_playedMissSFX && _enemiesAttackedIDs.Count == 0) {
                _fsm.PlayWeaponMissSFX();
                _playedMissSFX = true;
            }
            return;
        }

        if (DealDamage(hitEnemies, Vector2.down)) {
            // Utility.FreezePlayer(0.05f);
            _playerMovement.ApplyKnockback(Vector2.up, _fsm.weaponData.downthrustKnockBackForce, 0.05f);
            CameraShake.Instance.ShakeCamera(1f, 0.1f);
            _fsm.PlayWeaponHitSFX();
        }
    }

    bool DealDamage(Collider2D[] hitEnemies, Vector2 damageDir)
    {
        bool attacked = false;
        foreach (Collider2D hit in hitEnemies) {
            // Damage enemy/breakables only ONCE by adding them into list
            if (_enemiesAttackedIDs.Add (hit.gameObject.GetInstanceID ())) {
                EnemyFSM enemy = hit.GetComponent<EnemyFSM>();
                if (enemy != null && !enemy.IsDead()) {
                    enemy.TakeDamage(Constant.HAS_TIMED_COMBO ? _fsm.weaponData.comboDamage[(_playerAnimation.GetCurrentAttackAnimation() % _kMaxComboCount) - 1] : _fsm.weaponData.comboDamage[0],
                                     damageDir);
                    attacked = true;
                }

                BreakableObject breakable = hit.GetComponent<BreakableObject> ();
                if (breakable != null) {
                    breakable.TakeDamage(breakable.transform.position.x > _fsm.player.position.x);
                    attacked = true;
                }
            }
        }

        return attacked;
    }

    public void EndAttack()
    {
        // Next Attack handled in Update (cooldown)
        if (_hasNextBlock) {
            _fsm.SetState(_fsm.states[BowStateType.Parry]);
        }
        else if (_hasNextShoot) {
            _fsm.SetState(_fsm.states[BowStateType.Draw]);
        }
        else if (!_hasNextAttack) {
            _fsm.SetState(_fsm.states[BowStateType.Idle]);
        }

        _isListeningForNextAction = false;
        _enemiesAttackedIDs.Clear();
    }
}