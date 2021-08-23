using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiAttackState : WeaponState
{
    int _currentAttackCount;
    bool _isListeningForNextAttack;
    bool _stopListeningForNextAttack;
    bool _hasNextAttack;
    bool _playedMissSFX;
    const int _kMaxComboCount = 3;
    HashSet<int> _enemiesAttackedIDs;
    WeaponFSM _fsm;
    public override void EnterState(WeaponFSM fsm)
    {
        _fsm = fsm;
        fsm.movement.EnablePlayerMovement(false);
        fsm.attackCooldownTimer = fsm.weaponData.attackCooldown;
        _enemiesAttackedIDs = new HashSet<int> ();
        _currentAttackCount = 1;
        _isListeningForNextAttack = false;
        _stopListeningForNextAttack = false;
        _hasNextAttack = false;
        _playedMissSFX = false;

        BeginAttack();
    }

    public override void Update(WeaponFSM fsm)
    {
        if (Input.GetButtonDown ("Attack")) {
            if (_isListeningForNextAttack) {
                _hasNextAttack = true;
                _isListeningForNextAttack = false;
            } else {
                _stopListeningForNextAttack = true;
            }
        }
    }

    public override void FixedUpdate(WeaponFSM fsm)
    {
    }

    public override void ExitState(WeaponFSM fsm)
    {
        fsm.animations.EnablePlayerTurning(true);
        fsm.animations.SetAttackAnimation(0);
        fsm.movement.EnablePlayerMovement(true);
        _enemiesAttackedIDs.Clear();
    }
    
    void BeginAttack()
    {
        _fsm.animations.SetAttackAnimation(_currentAttackCount);
        _fsm.movement.StepForward(1f);
    }

    // Called from Animation frames
    public void StartListeningForNextAttackAndAttack()
    {
        if (!_stopListeningForNextAttack) {
            _isListeningForNextAttack = true;
        }
        Attack();
    }

    // Called from Animation frames
    public void Attack()
    {
        // Get Colliders of enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (_fsm.player.transform.TransformPoint(_fsm.weaponData.attackPoint), _fsm.weaponData.attackRange, 360, _fsm.weaponData.enemyLayers);

        if (hitEnemies.Length == 0) {
            if (!_playedMissSFX && _enemiesAttackedIDs.Count == 0) {
                _fsm.PlayWeaponMissSFX();
                _playedMissSFX = true;
            }
            return;
        }

        bool attacked = false;
        foreach (Collider2D hit in hitEnemies) {
            // Damage enemy/breakables only ONCE by adding them into list
            if (_enemiesAttackedIDs.Add (hit.gameObject.GetInstanceID ())) {
                EnemyFSM enemy = hit.GetComponent<EnemyFSM>();
                if (enemy != null && !enemy.IsDead()) {
                    enemy.TakeDamage(_fsm.weaponData.comboDamage[(_fsm.animations.GetCurrentAttackAnimation() % _kMaxComboCount) - 1]);
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
            // Freeze Frame effect
            Utility.StaticCoroutine.Start(Utility.ChangeVariableAfterDelay<float>(e => Time.timeScale = e, 0.1f, 0f, Time.timeScale));
            
            CameraShake.Instance.ShakeCamera(0.7f, 0.1f);
            Vector2 dir = new Vector2 (-_fsm.player.transform.localScale.x, 0.0f);
            _fsm.movement.ApplyKnockback(dir, _fsm.weaponData.horizontalKnockBackForce, 0f);
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
        else {
            _fsm.SetState(_fsm.states[WeaponFSM.StateType.IdleState]);
        }

        _stopListeningForNextAttack = false;
        _isListeningForNextAttack = false;
        _enemiesAttackedIDs.Clear ();
    }
}
