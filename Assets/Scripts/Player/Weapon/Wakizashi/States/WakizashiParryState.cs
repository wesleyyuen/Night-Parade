using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiParryState : IWeaponState
{
    HashSet<int> _enemiesParriedIDs = new HashSet<int> ();
    // LayerMask layers;
    bool _toIdle;

    public void Awake(WeaponFSM fsm)
    {
    }
    public void EnterState(WeaponFSM fsm)
    {
        // Ignore 'Breakables'
        // layers = fsm.weaponData.enemyLayers;
        // layers ^= (1 << LayerMask.GetMask("Breakables"));
        _toIdle = false;
        _enemiesParriedIDs.Clear();

        fsm.animations.SetBlockAnimation(true);
        fsm.animations.EnablePlayerTurning(false);
        fsm.movement.EnablePlayerMovement(false);
        fsm.blockCooldownTimer = fsm.weaponData.blockCooldown;
        fsm.currentBlockTimer = 0f;

        
        SpriteFlash flash = fsm.gameObject.GetComponent<SpriteFlash>();
        flash.PlayDamagedFlashEffect(fsm.weaponData.parryWindow);
    }

    public void Update(WeaponFSM fsm)
    {
        fsm.currentBlockTimer += Time.deltaTime;
        fsm.abilityController.UseStamina();
    
        if (fsm.abilityController.currStamina <= 0) {
            _toIdle = true;
            fsm.SetState(fsm.states[WeaponFSM.StateType.IdleState]);
        } else if (fsm.currentBlockTimer >= fsm.weaponData.parryWindow) {
            fsm.SetState(fsm.states[WeaponFSM.StateType.BlockState]);
        }

        Collider2D[] parried = Physics2D.OverlapBoxAll(fsm.player.transform.TransformPoint(fsm.weaponData.blockPoint), fsm.weaponData.blockRange, 360, LayerMask.GetMask("Enemies"));
        if (parried.Length == 0) return;

        foreach (Collider2D hit in parried) {
            // Parry enemy only ONCE by adding them into list
            if (_enemiesParriedIDs.Add (hit.gameObject.GetInstanceID ())) {
                EnemyFSM enemy = hit.GetComponent<EnemyFSM>();
                if (enemy != null && !enemy.IsDead())
                    fsm.Parry(enemy.transform.position.x < fsm.player.position.x, enemy);
            }
        }
    }

    public void FixedUpdate(WeaponFSM fsm)
    {
    }

    public void ExitState(WeaponFSM fsm)
    {
        fsm.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            fsm.animations.SetBlockAnimation(false);
        if (_toIdle) {
            fsm.movement.EnablePlayerMovement(true);
            fsm.animations.EnablePlayerTurning(true);
        }
    }
}
