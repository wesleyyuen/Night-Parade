using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;

public sealed class WakizashiAttackState : IWeaponState, IBindInput
{
    private InputManager _inputManager;
    private enum NextActionType
    {
        None,
        Attack,
        Throw
    }
    private readonly WakizashiFSM _fsm;
    private PlayerMovement _playerMovement;
    private PlayerAnimations _playerAnimation;
    private NextActionType _nextAction;
    private const int MAX_COMOBO_COUNT = 2;
    private const float ATTACK_STEP_DIST = 3f;
    private bool _isListeningForNextAction;
    private bool _playedMissSFX;
    private HashSet<int> _enemiesAttackedIDs;

    public WakizashiAttackState(WakizashiFSM fsm, InputManager inputManager)
    {
        _fsm = fsm;
        _inputManager = inputManager;

        _enemiesAttackedIDs = new HashSet<int>();
        _playerMovement = fsm.GetComponentInParent<PlayerMovement>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
    }

    public void BindInput()
    {
        _inputManager.Event_GameplayInput_Attack += OnNextAttack;
        _inputManager.Event_GameplayInput_ChargeAttack += OnChargeAttack;
    }

    public void UnbindInput()
    {
        _inputManager.Event_GameplayInput_Attack -= OnNextAttack;
        _inputManager.Event_GameplayInput_ChargeAttack -= OnChargeAttack;
    }

    public void EnterState()
    {
        if (Constant.STOP_WHEN_ATTACK)
            _playerMovement.EnablePlayerMovement(false);

        _fsm.attackCooldownTimer = _fsm.weaponData.attackCooldown;
        _nextAction = NextActionType.None;
        _enemiesAttackedIDs.Clear();
        _isListeningForNextAction = false;
        _playedMissSFX = false;

        // Begin First Attack
        // _playerAnimation.EnablePlayerTurning(false);
        _playerAnimation.SetAttackAnimation(1);
        _playerMovement.StepForward(ATTACK_STEP_DIST);
    }

    private void OnNextAttack()
    {
        // Listen for attack and queue as next action
        if (_fsm.IsCurrentState(WakizashiStateType.Attack) && _playerAnimation.GetCurrentAttackAnimation() > 0) {
            if (_isListeningForNextAction) {
                // _playerAnimation.EnablePlayerTurning(true);
                _nextAction = NextActionType.Attack;
                _isListeningForNextAction = false;
            }
        }
    }

    private void OnChargeAttack()
    {
        if (_fsm.IsCurrentState(WakizashiStateType.Attack)) {
        }
    }

    public void Update()
    {
        // Needed to constantly turn it off to avoid player moving after taking dmg
        if (Constant.STOP_WHEN_ATTACK) _playerMovement.EnablePlayerMovement(false);

        if (_nextAction == NextActionType.Attack && _fsm.attackCooldownTimer <= 0) {
            _fsm.attackCooldownTimer = _fsm.weaponData.attackCooldown;
            // _playerAnimation.EnablePlayerTurning(false);
            _nextAction = NextActionType.None;
            _playerAnimation.SetAttackAnimation((_playerAnimation.GetCurrentAttackAnimation() % MAX_COMOBO_COUNT) + 1);
            _playerMovement.StepForward(ATTACK_STEP_DIST);
            _playedMissSFX = false;
        }
    }

    public void FixedUpdate()
    {
    }

    public void Attack(bool isListeningForNextAttack)
    {
        _isListeningForNextAction = isListeningForNextAttack;

        // Get Colliders of enemies hit
        Vector2 attackPoint = new Vector2((_playerAnimation.IsFacingRight() ? 1f : -1f) * _fsm.weaponData.attackPoint.x, _fsm.weaponData.attackPoint.y);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(_fsm.player.transform.TransformPoint(attackPoint), _fsm.weaponData.attackRange, 360, _fsm.weaponData.enemyLayers);

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
            OnAttackHitCameraShake();
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
            OnAttackHitCameraShake();
            _fsm.PlayWeaponHitSFX();
        }
    }

    public void Downthrust(bool isListeningForNextAttack)
    {
        _isListeningForNextAction = isListeningForNextAttack;

        // Get Colliders of enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (_fsm.player.transform.TransformPoint(_fsm.weaponData.downthrustPoint), _fsm.weaponData.downthrustRange, 360, _fsm.weaponData.enemyLayers);

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
            OnAttackHitCameraShake();
            _fsm.PlayWeaponHitSFX();
        }
    }

    private bool DealDamage(Collider2D[] hitEnemies, Vector2 damageDir)
    {
        bool attacked = false;
        foreach (Collider2D hit in hitEnemies)
        {
            // Damage enemy/breakables only ONCE by adding them into list
            if (_enemiesAttackedIDs.Add(hit.gameObject.GetInstanceID())) {
                if (hit.TryGetComponent<IDamageable>(out IDamageable target)) {
                    if (target.TakeDamage(Constant.HAS_TIMED_COMBO ? _fsm.weaponData.comboDamage[(_playerAnimation.GetCurrentAttackAnimation() % MAX_COMOBO_COUNT) - 1] : _fsm.weaponData.comboDamage[0], damageDir)) {
                        attacked = true;
                    }
                    
                } 
            }
        }

        return attacked;
    }

    private void OnAttackHitCameraShake()
    {
        CameraShake.Instance.ShakeCamera(1.5f, 0.1f);
    }

    public void EndAttack()
    {
        _enemiesAttackedIDs.Clear();

        switch (_nextAction)
        {
            case NextActionType.Throw:
                _isListeningForNextAction = false;
                _fsm.SetState(_fsm.states[WakizashiStateType.Throw]);
                break;
            case NextActionType.Attack:
                _isListeningForNextAction = false;
                // Next Attack handled in Update (due to cooldown)
                return;
            case NextActionType.None:
                _isListeningForNextAction = true;
                break;
        }
    }

    public void OnNoNextAction()
    {
        // Debug.Log(_isListeningForNextAction);
        if (_isListeningForNextAction) {
            _fsm.SetState(_fsm.states[WakizashiStateType.Idle]);
        }
        else if (_nextAction == NextActionType.Throw) {
            _fsm.SetState(_fsm.states[WakizashiStateType.Throw]);
        }
    }

    public void ExitState()
    {
        // _playerAnimation.EnablePlayerTurning(true);
        _playerAnimation.SetAttackAnimation(0);
        _playerMovement.EnablePlayerMovement(true);
        _enemiesAttackedIDs.Clear();
    }
}
