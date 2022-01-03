using UnityEngine;

public class OkkaData : EnemyData
{
    public OkkaData()
    {
        // Health
        maxHealth = 14;
        currentHealth = maxHealth;

        // Patrol
        patrolSpeed = 1.5f;
        patrolDistance = 8f;

        // Aggression
        aggroDistance = 10f;
        aggroMovementSpeed = 5f;
        lineOfSightDistance = 15f;
        lineOfSightAngle = 25f;
        lineOfSightOriginOffset = new Vector2(0f, 0.4f);
        lineOfSightBreakDelay = 0.1f;
        timeFrozenAfterLOSBreak = 0.5f;

        // Attack
        damageAmount = 1;
        attackDistance = 3f;
        attackForce = 25f;
        attackChargeTime = 0.45f;
        attackTime = 1.25f;
        timeFrozenAfterDamagingPlayer = 1.25f;

        // Stunned
        knockBackOnParriedForce = 13f;
        timeStunnedAfterParried = 1.5f;
        knockBackOnBlockedForce = 6f;
        timeStunnedAfterBlocked = 0.6f;

        // Damaged
        knockBackOnTakingDamageForce = 10f;
        timeFrozenAfterTakingDamage = 0.15f;
        dieTime = 0.75f;

        // Spawn
        spawnCooldown = 90f;
    }
}
