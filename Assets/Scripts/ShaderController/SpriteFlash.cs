using System.Collections;
using UnityEngine;
using MEC;

// modified from Ilham Effendi
// https://github.com/ilhamhe/UnitySpriteFlash
public class SpriteFlash : MonoBehaviour
{
    [SerializeField] private Material _flashMaterial;
    [ColorUsage(true, true)]    // Enable HDR
    [SerializeField] private Color[] _flashColors;
	[SerializeField] private float _flashDuration;
    private int _flashColorIndex;
	private Material _material;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    private IEnumerator _coroutine;

    private void Awake()
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

        SetSpriteColor(_flashColors[_flashColorIndex]);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = _ActuallyFlash(duration == 0f ? _flashDuration : duration);
        StartCoroutine(_coroutine);
    }

    // For Enemy
    public void PlayDeathFlashEffect(float duration)
    {
        SetSpriteColor(new Color(1f, 1f, 1f, 0f));
        _spriteRenderer.color = Color.black;
        
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = _ActuallyFlash(duration);
        StartCoroutine(_coroutine);
    }

    public void SetSpriteColor(Color color)
    {
        StopAllCoroutines();
        _spriteRenderer.material = _material;
        _material.SetColor("_FlashColor", color);
        _material.SetFloat("_FlashAmount", 1f);
    }

    private IEnumerator _ActuallyFlash(float duration)
    {
        float lerpTime = 0;

        while (lerpTime < duration) {
            lerpTime += Time.deltaTime;
            float percent = lerpTime / duration;
            SetFlashAmount(1f - percent);
            yield return null;
        }

        SetFlashAmount(0);
        _spriteRenderer.material = _originalMaterial;
    }

    private IEnumerator _ActuallyFade(float duration)
    {
        float lerpTime = 0;

        while (lerpTime < duration) {
            lerpTime += Time.deltaTime;
            float percent = lerpTime / duration;
            SetFlashAmount(percent);
            yield return null;
        }

        _spriteRenderer.material = _originalMaterial;
        SetFlashAmount(0f);
    }

    private void SetFlashAmount(float amt)
    {
        _material.SetFloat("_FlashAmount", amt);
    }
}
