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

    public void Awake(WeaponFSM fsm)
    {
        _fsm = (WakizashiFSM) fsm;
        fsm.inputActions.Player.Block.canceled += OnReleaseBlock;
    }

    public void EnterState(WeaponFSM fsm)
    {
        // Ignore 'Breakables'
        // layers = fsm.weaponData.enemyLayers;
        // layers ^= (1 << LayerMask.GetMask("Breakables"));
        _isBlockReleasedBeforeMinDuration = false;
        _wasBlocking = false;

        fsm.animations.SetBlockAnimation(true);
        fsm.animations.EnablePlayerTurning(false);
        fsm.movement.EnablePlayerMovement(false);
    }

    void OnReleaseBlock(InputAction.CallbackContext context)
    {
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

        Collider2D[] blocked = Physics2D.OverlapBoxAll (fsm.player.transform.TransformPoint(fsm.weaponData.blockPoint), fsm.weaponData.blockRange, 360, LayerMask.GetMask("Enemies"));
        bool isBlocking = false;
        if (blocked.Length == 0) {
            _wasBlocking = isBlocking;
            return;
        } else
            isBlocking = true;

        foreach (Collider2D hit in blocked) {
            EnemyFSM enemy = hit.GetComponent<EnemyFSM>();
            if (enemy != null && !enemy.IsDead()) {
                Vector2 dir = new Vector2(fsm.player.transform.localScale.x, 0f);
                enemy.ApplyForce(dir, enemy.enemyData.knockBackOnBlockedForce, enemy.enemyData.timeStunnedAfterBlocked);
                enemy.StunForSeconds(enemy.enemyData.timeStunnedAfterBlocked);
            }
        }
        if (!_wasBlocking && isBlocking) {
            Vector2 dir = new Vector2(-fsm.player.transform.localScale.x, 0f);
            fsm.movement.ApplyKnockback(dir , fsm.weaponData.blockKnockback, 0.05f);
        }

        _wasBlocking = isBlocking;
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
