using UnityEngine;
public class WeaponData
{
    // Attack
    public int kMaxComboCount;
    public float[] comboDamage;
    public float attackCooldown;
    public LayerMask enemyLayers;

    // Hoirizontal
    public Vector2 attackPoint;
    public Vector2 attackRange;
    public float horizontalKnockBackForce;

    // Upthrust
    public Vector2 upthrustPoint;
    public Vector2 upthrustRange;
    public float upthrustKnockBackForce;

    // Downthrust
    public Vector2 downthrustPoint;
    public Vector2 downthrustRange;
    public float downthrustKnockBackForce;

    // Parry
    public float parryWindow;
    public float parryKnockback;

    // Block
    public Vector2 blockPoint;
    public Vector2 blockRange;
    public float blockMinDuration; // parryWindow < blockMinDuration
    public float blockKnockback;
    public float blockCooldown;

    // Throw
    public float throwForce;
    public float throwDamage;

    // Audio
    public string missSFX;
    public string hitSFX;
}
