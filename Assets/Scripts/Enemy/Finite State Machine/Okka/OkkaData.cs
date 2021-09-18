using UnityEngine;

public class OkkaData : EnemyData
{
    public OkkaData()
    {
        // Health
        maxHealth = 15;
        currentHealth = maxHealth;

        // Patrol
        patrolSpeed = 2f;
        patrolOriginOffset = 0.1f;

        // Aggression
        damageAmount = 1;
        aggroDistance = 7f;
        aggroMovementSpeed = 5f;
        lineOfSightDistance = 15f;
        lineOfSightAngle = 25f;
        lineOfSightOriginOffset = new Vector2(0f, 0.4f);
        lineOfSightBreakDelay = 0.1f;
        timeFrozenAfterLOSBreak = 0.5f;

        // Attack
        attackDistance = 4f;
        attackForce = 25f;
        attackChargeTime = 0.3f;
        attackTime = 0.6f;
        timeFrozenAfterDamagingPlayer = 1.5f;

        // Stunned
        knockBackOnParriedForce = 12f;
        timeStunnedAfterParried = 2.5f;
        knockBackOnBlockedForce = 6f;
        timeStunnedAfterBlocked = 0.4f;

        // Damaged
        knockBackOnTakingDamageForce = 10f;
        timeFrozenAfterTakingDamage = 0.5f;
        dieTime = 1f;
    }
}
