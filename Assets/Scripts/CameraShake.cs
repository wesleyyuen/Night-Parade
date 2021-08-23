using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance {get; private set;}
    CinemachineBasicMultiChannelPerlin perlin;
    float shakeTimer;
    float shakeTimerTotal;
    float startingIntensity;

    void Awake()
    {
        Instance = this;
        perlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        // Count down timer
        if (shakeTimer > 0f) {
            shakeTimer -= Time.deltaTime;

            // Stop shaking by smoothly decreasing intensity
            if (shakeTimer <= 0f) {
                perlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }

    public IEnumerator ShakeCameraAfterDelay(float delay, float intensity, float time)
    {
        yield return new WaitForSeconds(delay);
        ShakeCamera(intensity, time);
    }

    public void ShakeCamera(float intensity, float time)
    {
        // Set Camera Shake Intensity
        perlin.m_AmplitudeGain = intensity;

        startingIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }
}
