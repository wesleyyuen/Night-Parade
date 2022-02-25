using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealth", menuName = "ScriptableObjects/Player/PlayerHealthData")]
public class PlayerHealth : ScriptableObject
{
    public float damagedKnockBackForce;
    public float cameraShakeMultiplier;
    public float damageCameraShakeTimer;

}
