using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WakizashiBlockState : IWeaponState
{
    // LayerMask layers;
    bool _wasBlocking;
    bool _isBlockReleasedBeforeMinDuration;
    WakizashiFSM _fsm;
    PlayerAnimations _player;

    public void Awake(WeaponFSM fsm)
    {
        _fsm = (WakizashiFSM) fsm;
        fsm.inputActions.Player.Block.canceled += OnReleaseBlock;
    }

    public void EnterState(WeaponFSM fsm)
    {
        _player = fsm.player.GetComponent<PlayerAnimations>();
        _isBlockReleasedBeforeMinDuration = false;
        _wasBlocking = false;
        
        fsm.animations.SetBlockAnimation(true);
        fsm.animations.EnablePlayerTurning(false);
        fsm.movement.EnablePlayerMovement(false);
    }

    void OnReleaseBlock(InputAction.CallbackContext context)
    {
        if (_fsm == null) return;
        if (!_isBlockReleasedBeforeMinDuration && _fsm.currentBlockTimer >= _fsm.weaponData.blockMinDuration)
            _fsm.SetState(_fsm.states[WeaponFSM.StateType.IdleState]);
    }

    public void Update(WeaponFSM fsm)
    {
        fsm.currentBlockTimer += Time.deltaTime;
        fsm.abilityController.UseStamina();

        if (fsm.inputActions.Player.Block.ReadValue<float>() <= 0.5f && !_isBlockReleasedBeforeMinDuration && fsm.currentBlockTimer < fsm.weaponData.blockMinDuration) {
            _isBlockReleasedBeforeMinDuration = true;
        }

        if (fsm.abilityController.currStamina <= 0
            || (_isBlockReleasedBeforeMinDuration && fsm.currentBlockTimer >= fsm.weaponData.blockMinDuration)) {
            fsm.SetState(fsm.states[WeaponFSM.StateType.IdleState]);
        }

        Vector2 blockPoint = (Vector2)fsm.player.transform.TransformPoint(new Vector3(_player.GetPlayerScale().x * fsm.weaponData.blockPoint.x, fsm.weaponData.blockPoint.y));
        Collider2D[] blocked = Physics2D.OverlapAreaAll(blockPoint + new Vector2(-fsm.weaponData.blockRange.x/2, fsm.weaponData.blockRange.y/2),
                                                        blockPoint + new Vector2(fsm.weaponData.blockRange.x/2, -fsm.weaponData.blockRange.y/2),
                                                        LayerMask.GetMask("Enemies"));
        fsm.hasBlocked = false;
        if (blocked.Length == 0) {
            _wasBlocking = fsm.hasBlocked;
            return;
        } else
            fsm.hasBlocked = true;

        foreach (Collider2D hit in blocked) {
            EnemyFSM enemy = hit.GetComponent<EnemyFSM>();
            if (enemy != null && !enemy.IsDead()) {
                Vector2 dir = new Vector2(_player.GetPlayerScale().x, 0f);
                enemy.ApplyForce(dir, enemy.enemyData.knockBackOnBlockedForce, enemy.enemyData.timeStunnedAfterBlocked);
                enemy.StunForSeconds(enemy.enemyData.timeStunnedAfterBlocked);
            }
        }

        if (!_wasBlocking && fsm.hasBlocked) {
            Vector2 dir = new Vector2(_player.GetPlayerScale().x, 0f);
            fsm.movement.ApplyKnockback(-dir , fsm.weaponData.blockKnockback, 0.05f);
        }

        _wasBlocking = fsm.hasBlocked;
    }

    public void FixedUpdate(WeaponFSM fsm)
    {
    }

    public void ExitState(WeaponFSM fsm)
    {
        fsm.animations.SetBlockAnimation(false);
        fsm.animations.EnablePlayerTurning(true);
        fsm.movement.EnablePlayerMovement(true);
    }
}
