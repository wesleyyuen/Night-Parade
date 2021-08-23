using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakizashiData : WeaponData
{
    public WakizashiData()
    {
        // Attack
        kMaxComboCount = 3;
        comboDamage = new float[kMaxComboCount];
        comboDamage[0] = 5f;
        comboDamage[1] = 10f;
        comboDamage[2] = 15f;
        attackPoint = new Vector2(2.25f, 1.45f);
        attackRange = new Vector2(3f, 2.85f);
        attackCooldown = 0.2f;
        enemyLayers = LayerMask.GetMask("Enemies", "Breakables");
        horizontalKnockBackForce = 3f;

        // Parry
        parryWindow = 0.15f;
        parryKnockback = 7f;

        // Block
        blockPoint = new Vector2(0.95f, 1.45f);
        blockRange = new Vector2(0.4f, 2.85f);
        blockCooldown = 0.5f;
        blockMinDuration = 0.5f;  // parryWindow < blockMinDuration
        blockKnockback = 20f;

        // Audio
        missSFX = "Wakizashi_Miss";
        hitSFX = "Wakizashi_Hit";
    }
}
