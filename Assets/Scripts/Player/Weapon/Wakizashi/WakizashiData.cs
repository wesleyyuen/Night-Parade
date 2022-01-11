using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiData : WeaponData
{
    public readonly float throwVelocity;
    public readonly float throwMinDuration;
    public readonly float throwMaxDuration;
    public readonly float throwDamage;
    public readonly float throwCooldown;
    
    public WakizashiData()
    {
        // Attack
        kMaxComboCount = 3;
        comboDamage = new float[kMaxComboCount];
        comboDamage[0] = 5f;
        comboDamage[1] = 5f;
        comboDamage[2] = 5f;
        attackCooldown = 0.4f;
        enemyLayers = LayerMask.GetMask("Enemies", "Breakables");

        // Hoirizontal
        attackPoint = new Vector2(2.25f, 1.45f);
        attackRange = new Vector2(3f, 2.85f);
        horizontalKnockBackForce = 5f;

        // Upthrust
        upthrustPoint = new Vector2(0f, 4.25f);
        upthrustRange = new Vector2(3f, 2.85f);
        upthrustKnockBackForce = 5f;

        // Downthrust
        downthrustPoint = new Vector2(0f, -1.35f);
        downthrustRange = new Vector2(3f, 3f);
        downthrustKnockBackForce = 90f;

        // Parry
        parryWindow = 0.15f;
        parryDamage = 10f;
        parryKnockback = 10f;

        // Block
        blockPoint = new Vector2(0.95f, 1.45f);
        blockRange = new Vector2(0.4f, 2.85f);
        blockCooldown = 0.5f;
        blockMinDuration = 0.5f;  // parryWindow < blockMinDuration
        blockKnockback = 25f;

        // Throw
        throwVelocity = 65f;
        throwMinDuration = 0.15f;
        throwMaxDuration = 0.65f;
        throwDamage = 7f;
        throwCooldown = 0.7f;

        // Audio
        missSFX = "Wakizashi_Miss";
        hitSFX = "Wakizashi_Hit";
    }
}
