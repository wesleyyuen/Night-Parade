using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] float _maxReduction = 0.2f;
    [SerializeField] float _maxIncrease = 0.2f;
    // [SerializeField] float _rateDamping = 0.3f;
    [SerializeField] float _strength = 300f;
    bool _stopFlickering;
    Light2D _lightSource;
    float _baseIntensity;
 
    void Awake()
    {
        _lightSource = GetComponent<Light2D>();
        _baseIntensity = _lightSource.intensity;
        StartCoroutine(DoFlicker());
    }

    IEnumerator DoFlicker()
    {
        while (!_stopFlickering) {
            _lightSource.intensity = Mathf.SmoothStep(_lightSource.intensity, Mathf.Max(0f, Random.Range(_baseIntensity - _maxReduction, _baseIntensity + _maxIncrease)), _strength * Time.deltaTime);
            yield return null;
        }
    }
}
