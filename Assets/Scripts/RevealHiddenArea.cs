using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RevealHiddenArea : MonoBehaviour
{
    [SerializeField] string keyString;
    [SerializeField] Tilemap tilemapToHide;
    [SerializeField] GameObject lightToTurnOff;
    [SerializeField] float alphaToFadeTo;
    [SerializeField] float fadingTime;
    [SerializeField] bool hideFromLeft;
    PlayerProgress _progress;

    void Start ()
    {
        _progress = FindObjectOfType<PlayerProgress>();
        // Make hidden area semi-transparant when discovered before
        if (_progress.HasPlayerProgress(keyString)) {
            tilemapToHide.color = new Color (1, 1, 1, alphaToFadeTo);
            Destroy(lightToTurnOff);
        }
    }

    void OnTriggerExit2D (Collider2D other)
    {
        if (other.CompareTag ("Player")) {
            // Find direction player enter the trigger from
            Vector3 dir = other.transform.position - gameObject.transform.position;
            if (hideFromLeft && dir.x < 0 || !hideFromLeft && dir.x > 0) {
                StartCoroutine (FadeTilemap (alphaToFadeTo));
            } else {
                // Only play sound effect if First time walking into hidden area
                if (!_progress.HasPlayerProgress(keyString)) {
                    _progress.AddPlayerProgress(keyString, 1);
                    FoundSecretSFX ();
                }
                StartCoroutine (FadeTilemap (0f));
            }
        }
    }

    // Fade in/out tilemap
    IEnumerator FadeTilemap (float targetAlpha)
    {
        if (lightToTurnOff != null) lightToTurnOff.SetActive (false);
        float alpha = tilemapToHide.color.a;
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            tilemapToHide.color = new Color (1, 1, 1, Mathf.Lerp (alpha, targetAlpha, t));
            yield return null;
        }
    }

    void FoundSecretSFX ()
    {
        SoundManager.Instance.Play ("Found_SecretArea");
    }
}