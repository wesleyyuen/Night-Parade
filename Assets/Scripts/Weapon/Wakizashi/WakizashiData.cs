using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WakizashiData", menuName = "ScriptableObjects/Player/Weapon/WakizashiData")]
public class WakizashiData : WeaponData
{
    [Header("Throw")]
    public float throwVelocity;
    public float throwMinDuration;
    public float throwDamage;
    public float throwCooldown;
}
