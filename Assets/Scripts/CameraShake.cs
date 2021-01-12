using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour {

    public static CameraShake Instance {get; private set;}
    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingIntensity;

    private void Awake () {
        Instance = this;
        perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update () {
        // Count down timer
        if (shakeTimer > 0f) {
            shakeTimer -= Time.deltaTime;

            // Stop shaking by smoothly decreasing intensity
            if (shakeTimer <= 0f) {
                perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }

    public void ShakeCamera (float intensity, float time) {
        // Set Camera Shake Intensity
        perlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }
    
}
