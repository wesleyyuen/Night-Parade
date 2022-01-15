using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class WakizashiParryState : IWeaponState
{
    private WakizashiFSM _fsm;
    private HashSet<int> _enemiesParriedIDs;
    private PlayerMovement _playerMovement;
    private PlayerAnimations _playerAnimation;
    private PlayerAbilityController _abilityController;
    private SpriteFlash _flash;
    private bool _toIdle;

    public WakizashiParryState(WakizashiFSM fsm)
    {
        _fsm = fsm;

        _enemiesParriedIDs = new HashSet<int>();
        _abilityController = fsm.GetComponentInParent<PlayerAbilityController>();
        _playerMovement = fsm.GetComponentInParent<PlayerMovement>();
        _playerAnimation = fsm.GetComponentInParent<PlayerAnimations>();
        _flash = fsm.gameObject.GetComponent<SpriteFlash>();
    }

    public void EnterState()
    {
        _toIdle = false;
        _enemiesParriedIDs.Clear();

        _playerAnimation.SetBlockAnimation(true);
        _playerAnimation.EnablePlayerTurning(false);
        _playerMovement.EnablePlayerMovement(false);
        _fsm.blockCooldownTimer = _fsm.weaponData.blockCooldown;
        _fsm.currentBlockTimer = 0f;
        
        _flash.PlayDamagedFlashEffect(_fsm.weaponData.parryWindow);
    }

    public void Update()
    {
        _fsm.currentBlockTimer += Time.deltaTime;
        _abilityController.UseStamina();
    
        if (_abilityController.currStamina <= 0) {
            _toIdle = true;
            _fsm.SetState(_fsm.states[WakizashiStateType.Idle]);
        } else if (_fsm.currentBlockTimer >= _fsm.weaponData.parryWindow) {
            _fsm.SetState(_fsm.states[WakizashiStateType.Block]);
        }

        Vector2 parryPoint = (Vector2) _fsm.player.transform.TransformPoint(new Vector3((_playerAnimation.IsFacingRight() ? 1f : -1f) * _fsm.weaponData.blockPoint.x, _fsm.weaponData.blockPoint.y));
        Collider2D[] parried = Physics2D.OverlapAreaAll(parryPoint + new Vector2(-_fsm.weaponData.blockRange.x/2, _fsm.weaponData.blockRange.y/2),
                                                        parryPoint + new Vector2(_fsm.weaponData.blockRange.x/2, -_fsm.weaponData.blockRange.y/2),
                                                        LayerMask.GetMask("Enemies"));
        _fsm.hasBlocked = false;
        if (parried.Length == 0) return;
        _fsm.hasBlocked = true;

        TimeManager.Instance.SlowTimeForSeconds(0.3f, 0.5f);

        foreach (Collider2D hit in parried) {
            // Parry enemy only ONCE by adding them into list
            if (_enemiesParriedIDs.Add (hit.gameObject.GetInstanceID ())) {
                if (hit.TryGetComponent<EnemyFSM>(out EnemyFSM enemy) && !enemy.IsDead()) {
                    Vector2 hitDir = _playerAnimation.IsFacingRight() ? Vector2.right : Vector2.left;
                    // enemy.TakeDamage(_fsm.weaponData.parryDamage, hitDir);

                    enemy.ApplyForce(-hitDir, enemy.enemyData.knockBackOnParriedForce, enemy.enemyData.timeStunnedAfterParried);
                    enemy.StunForSeconds(enemy.enemyData.timeStunnedAfterParried);

                    // TODO: parry animation

                    _fsm.SetStateAfterDelay(WakizashiStateType.Idle, 1f);
                }
            }
        }
    }

    public void FixedUpdate()
    {
    }

    public void ExitState()
    {
        _playerAnimation.SetBlockAnimation(false);
        _fsm.hasBlocked = false;
        if (_toIdle) {
            _playerMovement.EnablePlayerMovement(true);
            _playerAnimation.EnablePlayerTurning(true);
        }
    }
}
