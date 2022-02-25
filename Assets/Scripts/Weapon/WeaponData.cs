using UnityEngine;

public class WeaponData : ScriptableObject
{
    [Header("Attack")]
    public int maxComboCount;
    public float[] comboDamage;
    public float attackCooldown;
    public LayerMask enemyLayers;

    [Header("Horizontal Attack")]
    public Vector2 attackPoint;
    public Vector2 attackRange;
    public float horizontalKnockBackForce;

    [Header("Upthrust Attack")]
    public Vector2 upthrustPoint;
    public Vector2 upthrustRange;
    public float upthrustKnockBackForce;

    [Header("Downthrust Attack")]
    public Vector2 downthrustPoint;
    public Vector2 downthrustRange;
    public float downthrustKnockBackForce;

    [Header("Parry")]
    public float parryWindow;
    public float parryKnockback;

    [Header("Block")]
    public Vector2 blockPoint;
    public Vector2 blockRange;
    public float blockMinDuration; // parryWindow < blockMinDuration
    public float blockKnockback;
    public float blockCooldown;

    [Header("Audio")]
    public AudioEvent missSFX;
    public AudioEvent hitSFX;
}