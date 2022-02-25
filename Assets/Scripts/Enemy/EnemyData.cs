using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Health")]
    public float maxHealth;

    [Header("Patrol Behavior")]
    public float patrolSpeed;
    public float patrolDistance;

    [Header("Aggressive Behavior")]
    public float aggroMovementSpeed;
    public float noLOSAggroDistance;
    public float lineOfSightDistance;
    public float lineOfSightAngle;
    public Vector2 lineOfSightOriginOffset;
    public float lineOfSightBreakDelay;
    public float timeStunnedAfterLOSBreak;

    [Header("Attack Behavior")]
    public int damageAmount;
    public float attackDistance;
    public float attackForce;
    public float attackChargeTime;
    public float attackTime; // Excluding charge Time
    public float timeStunnedAfterDamagingPlayer;

    [Header("Stunned Behavior")]
    public float knockBackOnParriedForce;
    public float timeStunnedAfterParried;
    public float knockBackOnBlockedForce;
    public float timeStunnedAfterBlocked;


    [Header("Damaged Behavior")]
    public float knockBackOnTakingDamageForce;
    public float timeStunnedAfterTakingDamage;
    public float dieTime; // in seconds

    [Header("Respawn Duration")]
    public float spawnCooldown; // in seconds

    [Header("Drops")]
    public Vector2 dropSpawnOffset;
    public Vector2 dropSpawnForce;
    public GameObject mon;
    public int numOfCoinsDrop;
    [Range(0, 1)] public float coinsDropPercent;
    public GameObject heart;
    public int numOfHeartsDrop;
    [Range(0, 1)] public float heartDropPercent;
    public Optional<GameObject> specialDrop;
    public Vector2 specialDropSpawnOffset;

    public void SpawnDrops(Vector3 spawnOrigin)
    {
        // Drop Coins
        if (Random.value > 1 - coinsDropPercent) {
            for (int i = 0; i < numOfCoinsDrop; i++) {
                GameObject coin = Instantiate(mon, spawnOrigin + (Vector3) dropSpawnOffset, Quaternion.identity);
                coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(dropSpawnForce.x * Random.Range(-1, 1), dropSpawnForce.y * Random.Range(0.4f, 1)));
            }
        }

        // Drop Health
        if (Random.value > 1 - heartDropPercent) {
            for (int i = 0; i < numOfHeartsDrop; i++) {
                GameObject health = Instantiate(heart, spawnOrigin + (Vector3) dropSpawnOffset, Quaternion.identity);
                health.GetComponent<Rigidbody2D>().AddForce(new Vector2(dropSpawnForce.x * Random.Range(-1, 1), dropSpawnForce.y * Random.Range(0.4f, 1)));
            }
        }

        // Special Drop
        if (specialDrop.Enabled) {
            Instantiate(specialDrop.Value, spawnOrigin + (Vector3) specialDropSpawnOffset, Quaternion.identity);
        }
    }
}
