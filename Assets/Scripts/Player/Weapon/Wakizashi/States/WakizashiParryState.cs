using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiParryState : IWeaponState
{
    HashSet<int> _enemiesParriedIDs = new HashSet<int>();
    PlayerAnimations _player;
    SpriteFlash _flash;
    bool _toIdle;

    public void Awake(WeaponFSM fsm)
    {
        _player = fsm.player.GetComponent<PlayerAnimations>();
        _flash = fsm.gameObject.GetComponent<SpriteFlash>();
    }
    public void EnterState(WeaponFSM fsm)
    {
        _toIdle = false;
        _enemiesParriedIDs.Clear();

        fsm.animations.SetBlockAnimation(true);
        fsm.animations.EnablePlayerTurning(false);
        fsm.movement.EnablePlayerMovement(false);
        fsm.blockCooldownTimer = fsm.weaponData.blockCooldown;
        fsm.currentBlockTimer = 0f;
        
        // SpriteFlash flash = fsm.gameObject.GetComponent<SpriteFlash>();
        _flash.PlayDamagedFlashEffect(fsm.weaponData.parryWindow);
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

        Vector2 parryPoint = (Vector2)fsm.player.transform.TransformPoint(new Vector3(_player.GetPlayerScale().x * fsm.weaponData.blockPoint.x, fsm.weaponData.blockPoint.y));
        Collider2D[] parried = Physics2D.OverlapAreaAll(parryPoint + new Vector2(-fsm.weaponData.blockRange.x/2, fsm.weaponData.blockRange.y/2),
                                                        parryPoint + new Vector2(fsm.weaponData.blockRange.x/2, -fsm.weaponData.blockRange.y/2),
                                                        LayerMask.GetMask("Enemies"));
        fsm.hasBlocked = false;
        if (parried.Length == 0) return;
        fsm.hasBlocked = true;

        foreach (Collider2D hit in parried) {
            // Parry enemy only ONCE by adding them into list
            if (_enemiesParriedIDs.Add (hit.gameObject.GetInstanceID ())) {
                EnemyFSM enemy = hit.GetComponent<EnemyFSM>();
                if (enemy != null && !enemy.IsDead()) {
                    Utility.StaticCoroutine.Start(Utility.ChangeVariableAfterDelayInRealTime<float>(e => Time.timeScale = e, 0.6f, 0.15f, 1f));

                    Vector2 dir = new Vector2 (enemy.transform.position.x < fsm.player.position.x ? 1f : -1f, 0f);
                    enemy.ApplyForce(-dir, enemy.enemyData.knockBackOnParriedForce, enemy.enemyData.timeStunnedAfterParried);
                    enemy.StunForSeconds(enemy.enemyData.timeStunnedAfterParried);

                    fsm.SetStateAfterDelay(WeaponFSM.StateType.IdleState, 1f);
                }
            }
        }
    }

    public void FixedUpdate(WeaponFSM fsm)
    {
    }

    public void ExitState(WeaponFSM fsm)
    {
        fsm.animations.SetBlockAnimation(false);
        fsm.hasBlocked = false;
        if (_toIdle) {
            fsm.movement.EnablePlayerMovement(true);
            fsm.animations.EnablePlayerTurning(true);
        }
    }
}
