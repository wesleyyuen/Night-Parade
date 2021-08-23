using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    // Health
    public float maxHealth;
    public float currentHealth;

    // Patrol
    public float patrolSpeed;
    public float patrolOriginOffset;

    // Aggression
    public int damageAmount;
    public float aggroDistance;
    public float aggroMovementSpeed;
    public float lineOfSightDistance;
    public float lineOfSightAngle;
    public Vector2 lineOfSightOriginOffset;
    public float lineOfSightBreakDelay;
    public float timeFrozenAfterLOSBreak;
    public float timeFrozenAfterDamagingPlayer;

    // Damaged
    public float knockBackOnAttackedForce;
    public float knockBackOnParriedForce;
    public float timeStunnedAfterParried;
    public float timeFrozenAfterTakingDamage;
    public float dieTime;
}
