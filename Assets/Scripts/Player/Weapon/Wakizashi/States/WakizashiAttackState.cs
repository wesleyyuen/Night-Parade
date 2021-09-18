using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WakizashiAttackState : IWeaponState
{
    const int _kMaxComboCount = 2;
    int _currentAttackCount;
    bool _isListeningForNextAttack;
    bool _hasNextAttack;
    bool _hasNextBlock;
    bool _playedMissSFX;
    HashSet<int> _enemiesAttackedIDs;
    WakizashiFSM _fsm;

    public void Awake(WeaponFSM fsm)
    {
        _fsm = (WakizashiFSM) fsm;
        fsm.inputActions.Player.Attack.started += OnNextAttack;
        fsm.inputActions.Player.Attack.performed += OnNextAttack;
        fsm.inputActions.Player.Block.started += OnNextBlock;
    }

    public void EnterState(WeaponFSM fsm)
    {
        fsm.movement.EnablePlayerMovement(false);
        fsm.EnablePlayerBlocking(false);
        // fsm.movement.ChangePlayerMovementSpeed(true, 0.5f, 0f);
        fsm.attackCooldownTimer = fsm.weaponData.attackCooldown;
        _enemiesAttackedIDs = new HashSet<int> ();
        _isListeningForNextAttack = false;
        _hasNextAttack = false;
        _hasNextBlock = false;
        _playedMissSFX = false;

        // Begin First Attack
        _currentAttackCount = 1;
        fsm.animations.SetAttackAnimation(_currentAttackCount);
        fsm.movement.StepForward(1f);
    }

    void OnNextAttack(InputAction.CallbackContext context)
    {
        // Only fire on consecutive attacks
        if (context.started && _currentAttackCount > 0) {
            if (_isListeningForNextAttack) {
                _hasNextAttack = true;
                _isListeningForNextAttack = false;
            }
        }
        // else if (context.performed) {
        //     OnChargeAttack();
        // }
    }

    void OnNextBlock(InputAction.CallbackContext context)
    {
        // Listen for block and queue as next action
        if (context.started && _currentAttackCount > 0) {
            if (_isListeningForNextAttack) {
                _hasNextBlock = true;
                _isListeningForNextAttack = false;
            }
        }
        // else if (context.performed) {
        //     OnChargeAttack();
        // }
    }

    void OnChargeAttack()
    {
        Utility.StaticCoroutine.Start(_fsm.MergeWithOnibi(1f));
    }

    public void Update(WeaponFSM fsm)
    {
    }

    public void FixedUpdate(WeaponFSM fsm)
    {
    }

    public void ExitState(WeaponFSM fsm)
    {
        fsm.animations.EnablePlayerTurning(true);
        fsm.EnablePlayerBlocking(true);
        fsm.animations.SetAttackAnimation(0);
        // fsm.movement.ChangePlayerMovementSpeed(false, 1f, 0f);
        fsm.movement.EnablePlayerMovement(true);
        _enemiesAttackedIDs.Clear();
    }

    // Called from Animation frames
    public void Attack(bool isListeningForNextAttack)
    {
        _isListeningForNextAttack = isListeningForNextAttack;

        // Get Colliders of enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (_fsm.player.transform.TransformPoint(_fsm.weaponData.attackPoint), _fsm.weaponData.attackRange, 360, _fsm.weaponData.enemyLayers);

        // No Hits
        if (hitEnemies.Length == 0) {
            if (!_playedMissSFX && _enemiesAttackedIDs.Count == 0) {
                _fsm.PlayWeaponMissSFX();
                _playedMissSFX = true;
            }
            return;
        }

        // 1+ Hits
        bool attacked = false;
        foreach (Collider2D hit in hitEnemies) {
            // Damage enemy/breakables only ONCE by adding them into list
            if (_enemiesAttackedIDs.Add (hit.gameObject.GetInstanceID ())) {
                EnemyFSM enemy = hit.GetComponent<EnemyFSM>();
                if (enemy != null && !enemy.IsDead()) {
                    enemy.TakeDamage(Constant.hasCombo ? _fsm.weaponData.comboDamage[(_fsm.animations.GetCurrentAttackAnimation() % _kMaxComboCount) - 1] : _fsm.weaponData.comboDamage[0]);
                    attacked = true;
                }

                BreakableObject breakable = hit.GetComponent<BreakableObject> ();
                if (breakable != null) {
                    breakable.TakeDamage (breakable.transform.position.x > _fsm.player.position.x);
                    attacked = true;
                }
            }
        }

        if (attacked) {           
            Utility.FreezePlayer(0.05f);
            CameraShake.Instance.ShakeCamera(1f, 0.1f);
            // Vector2 dir = new Vector2 (-_fsm.player.transform.localScale.x, 0.0f);
            // _fsm.movement.ApplyKnockback(dir, _fsm.weaponData.horizontalKnockBackForce, 0.1f);
            _fsm.PlayWeaponHitSFX();
        }
    }

    // Called from animation frame
    public void EndAttack ()
    {
        if (_hasNextAttack) {
            _currentAttackCount = (_currentAttackCount % _kMaxComboCount) + 1;
            _fsm.animations.SetAttackAnimation(_currentAttackCount);
            _fsm.movement.StepForward(1);
            _hasNextAttack = false;
            _playedMissSFX = false;
        }
        else if (_hasNextBlock) {
            _fsm.SetState(_fsm.states[WeaponFSM.StateType.ParryState]);
        }
        else {
            _fsm.SetState(_fsm.states[WeaponFSM.StateType.IdleState]);
        }

        _isListeningForNextAttack = false;
        _enemiesAttackedIDs.Clear ();
    }
}
