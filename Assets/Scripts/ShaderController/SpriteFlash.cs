using System.Collections;
using UnityEngine;

// modified from Ilham Effendi
// https://github.com/ilhamhe/UnitySpriteFlash
public class SpriteFlash : MonoBehaviour
{
    [SerializeField] Material flashMaterial;
    [SerializeField] Color[] flashColor;
	[SerializeField] float flashDuration;
    int flashColorIndex;
	Material mat;
    SpriteRenderer render;
    Material originalMaterial;
    IEnumerator coroutine;

    void Awake()
    {
        flashColorIndex = 0;
        render = GetComponent<SpriteRenderer>();
        originalMaterial = render.material;
        mat = new Material(flashMaterial);
        mat.SetColor("_FlashColor", flashColor[flashColorIndex]);
    }

    public void PlayDamagedFlashEffect()
    {
        // Set Next Color
        flashColorIndex++;
        flashColorIndex = flashColorIndex % flashColor.Length;
        mat.SetColor("_FlashColor", flashColor[flashColorIndex]);

        render.material = mat;

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = ActuallyPlayDamagedFlashEffect();
        StartCoroutine(coroutine);
    }

    public void PlayDeathFlashEffect(float duration)
    {
        mat.SetColor("_FlashColor", flashColor[flashColorIndex]);
        render.material = mat;
        
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = ActuallyFade(duration);
        StartCoroutine(coroutine);
    }

    IEnumerator ActuallyPlayDamagedFlashEffect()
    {
        float lerpTime = 0;

        while (lerpTime < flashDuration) {
            // Set Alpha
            lerpTime += Time.deltaTime;
            float percent = lerpTime / flashDuration;
            mat.SetFloat("_FlashAmount", 1f - percent);
            yield return null;
        }

        render.material = originalMaterial;
        mat.SetFloat("_FlashAmount", 0);
    }

    IEnumerator ActuallyFlash()
    {
        float lerpTime = 0;

        while (lerpTime < flashDuration) {
            lerpTime += Time.deltaTime;
            float percent = lerpTime / flashDuration;
            mat.SetFloat("_FlashAmount", 1f - percent);
            yield return null;
        }

        render.material = originalMaterial;
        mat.SetFloat("_FlashAmount", 0);
    }

    IEnumerator ActuallyFade(float duration)
    {
        float lerpTime = 0;

        while (lerpTime < duration) {
            lerpTime += Time.deltaTime;
            float percent = lerpTime / duration;
            mat.SetFloat("_FlashAmount", percent);
            yield return null;
        }

        render.material = originalMaterial;
        mat.SetFloat("_FlashAmount", 0);
    }
}
