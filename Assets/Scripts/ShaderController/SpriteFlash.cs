using System.Collections;
using UnityEngine;

// modified from Ilham Effendi
// https://github.com/ilhamhe/UnitySpriteFlash
public class SpriteFlash : MonoBehaviour
{
    [SerializeField] Material _flashMaterial;
    [SerializeField] Color[] _flashColors;
	[SerializeField] float _flashDuration;
    int _flashColorIndex;
	Material _material;
    [SerializeField] SpriteRenderer _spriteRenderer;
    Material _originalMaterial;
    IEnumerator _coroutine;

    void Awake()
    {
        _flashColorIndex = 0;
        _originalMaterial = _spriteRenderer.material;
        _material = new Material(_flashMaterial);
        _material.SetColor("_FlashColor", _flashColors[_flashColorIndex]);
    }

    public void PlayDamagedFlashEffect(float duration = 0f)
    {
        // Set Next Color
        _flashColorIndex++;
        _flashColorIndex = _flashColorIndex % _flashColors.Length;
        _material.SetColor("_FlashColor", _flashColors[_flashColorIndex]);

        _spriteRenderer.material = _material;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = _ActuallyFlash(duration == 0 ? _flashDuration : duration);
        StartCoroutine(_coroutine);
    }

    // For Enemy
    public void PlayDeathFlashEffect(float duration)
    {
        _material.SetColor("_FlashColor", new Color(1f, 1f, 1f, 0f));
        _spriteRenderer.material = _material;
        _spriteRenderer.color = Color.black;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = _ActuallyFlash(duration);
        StartCoroutine(_coroutine);
    }

    IEnumerator _ActuallyFlash(float duration)
    {
        float lerpTime = 0;

        while (lerpTime < duration) {
            lerpTime += Time.deltaTime;
            float percent = lerpTime / duration;
            _material.SetFloat("_FlashAmount", 1f - percent);
            yield return null;
        }

        _spriteRenderer.material = _originalMaterial;
        _material.SetFloat("_FlashAmount", 0);
    }

    IEnumerator _ActuallyFade(float duration)
    {
        float lerpTime = 0;

        while (lerpTime < duration) {
            lerpTime += Time.deltaTime;
            float percent = lerpTime / duration;
            _material.SetFloat("_FlashAmount", percent);
            yield return null;
        }

        _spriteRenderer.material = _originalMaterial;
        _material.SetFloat("_FlashAmount", 0);
    }
}
