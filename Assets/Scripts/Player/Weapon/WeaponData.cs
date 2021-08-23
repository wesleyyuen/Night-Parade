using UnityEngine;
public class WeaponData
{
    // Attack
    public int kMaxComboCount;
    public float[] comboDamage;
    public float attackCooldown;
    public Vector2 attackPoint;
    public Vector2 attackRange;
    public LayerMask enemyLayers;
    public float horizontalKnockBackForce;

    // Parry
    public float parryWindow;
    public float parryKnockback;

    // Block
    public Vector2 blockPoint;
    public Vector2 blockRange;
    public float blockMinDuration; // parryWindow < blockMinDuration
    public float blockKnockback;
    public float blockCooldown;

    // Audio
    public string missSFX;
    public string hitSFX;
}
