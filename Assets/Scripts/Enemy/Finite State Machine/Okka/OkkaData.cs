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
        aggroMovementSpeed = 5f;
        aggroDistance = 5f;
        lineOfSightDistance = 15f;
        lineOfSightAngle = 25f;
        lineOfSightOriginOffset = new Vector2(0f, 0.4f);
        lineOfSightBreakDelay = 0.1f;
        timeFrozenAfterLOSBreak = 0.5f;
        timeFrozenAfterDamagingPlayer = 1.5f;

        // Damaged
        knockBackOnAttackedForce = 10f;
        knockBackOnParriedForce = 12f;
        timeStunnedAfterParried = 2.5f;
        timeFrozenAfterTakingDamage = 0.5f;
        dieTime = 1.0f;
    }
}
