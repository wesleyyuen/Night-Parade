using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiBlockState : WeaponState
{
    // LayerMask layers;
    bool _wasBlocking;
    bool _isBlockReleasedBeforeMinDuration;
    public override void EnterState(WeaponFSM fsm)
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

    public override void Update(WeaponFSM fsm)
    {
        fsm.currentBlockTimer += Time.deltaTime;
        fsm.abilityController.UseStamina();

        if (!Input.GetButton("Block") && !_isBlockReleasedBeforeMinDuration && fsm.currentBlockTimer < fsm.weaponData.blockMinDuration) {
            _isBlockReleasedBeforeMinDuration = true;
        }

        if (fsm.abilityController.currStamina <= 0
            || (Input.GetButtonUp("Block") && !_isBlockReleasedBeforeMinDuration && fsm.currentBlockTimer >= fsm.weaponData.blockMinDuration)
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
            if (enemy != null && !enemy.IsDead() && !enemy.IsCurrentState(EnemyFSM.StateType.StillState)) {
                enemy.SetState(enemy.states[EnemyFSM.StateType.StillState]);
            }
        }
        if (!_wasBlocking && isBlocking) {
            Vector2 dir = new Vector2(-fsm.player.transform.localScale.x, 0f);
            fsm.movement.ApplyKnockback(dir , fsm.weaponData.blockKnockback, 0.05f);
        }

        _wasBlocking = isBlocking;
    }

    public override void FixedUpdate(WeaponFSM fsm)
    {
    }

    public override void ExitState(WeaponFSM fsm)
    {
        fsm.animations.SetBlockAnimation(false);
        fsm.animations.EnablePlayerTurning(true);
        fsm.movement.EnablePlayerMovement(true);
    }
}
