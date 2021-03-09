using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by Ilham Effendi
// https://github.com/ilhamhe/UnitySpriteFlash
public class SpriteFlash : MonoBehaviour {
    [SerializeField] private Color flashColor;
	[SerializeField] private float flashDuration;
	private Material mat;
    private Enemy enemy;
    private IEnumerator coroutine;

    private void Awake() {
        mat = GetComponent<SpriteRenderer>().material;
        enemy = GetComponent<Enemy>();
    }

    private void Start() {
        mat.SetColor("_FlashColor", flashColor);
    }

    public void Flash() {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = ActuallyFlash();
        StartCoroutine(coroutine);
    }

    private IEnumerator ActuallyFlash() {
        float lerpTime = 0;

        while (lerpTime < flashDuration) {
            lerpTime += Time.deltaTime;
            float percent = lerpTime / flashDuration;
            mat.SetFloat("_FlashAmount", 1f - percent);
            yield return null;
        }

        mat.SetFloat("_FlashAmount", 0);
        enemy.isTakingDmg = false;
    }
}
