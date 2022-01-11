using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;
    public static CameraShake Instance {
        get  {return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    public void ShakeCamera(float intensity, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(_ShakeCameraCoroutine(intensity, duration));
    }

    public void ShakeCameraAfterDelay(float delay, float intensity, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(_ShakeCameraAfterDelayCoroutine(delay, intensity, duration));
    }

    private IEnumerator _ShakeCameraAfterDelayCoroutine(float delay, float intensity, float duration)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(_ShakeCameraCoroutine(intensity, duration));
    }

    private IEnumerator _ShakeCameraCoroutine(float intensity, float duration)
    {
        CinemachineBrain brain = CinemachineCore.Instance.GetActiveBrain(0);
        CinemachineVirtualCamera vcam = brain.ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();
        CinemachineBasicMultiChannelPerlin perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // Set Camera Shake Intensity
        perlin.m_AmplitudeGain = intensity;

        float elapsedTime = 0f;

        yield return new WaitForSeconds(duration);
        
        while (elapsedTime < duration)
        {
            perlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        perlin.m_AmplitudeGain = 0f;
    }
}
