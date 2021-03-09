using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RevealHiddenArea : MonoBehaviour {
    [SerializeField] private string keyString;
    [SerializeField] private Tilemap tilemapToHide;
    [SerializeField] private GameObject lightToTurnOff;
    [SerializeField] private float alphaToFadeTo;
    [SerializeField] private float fadingTime;
    [SerializeField] private bool hideFromLeft;

    private void Start () {
        // Make hidden area semi-transparant when discovered before
        bool discoveredBefore;
        FindObjectOfType<PlayerProgress> ().areaProgress.TryGetValue (keyString, out discoveredBefore);
        if (discoveredBefore) {
            tilemapToHide.color = new Color (1, 1, 1, alphaToFadeTo);
            Destroy (lightToTurnOff);
        }
    }

    private void OnTriggerExit2D (Collider2D other) {
        if (other.CompareTag ("Player")) {
            // Find direction player enter the trigger from
            Vector3 dir = other.transform.position - gameObject.transform.position;
            if (hideFromLeft && dir.x < 0 || !hideFromLeft && dir.x > 0) {
                StartCoroutine (FadeTilemap (alphaToFadeTo));
            } else {
                // Only play sound effect if First time walking into hidden area
                bool temp;
                if (!FindObjectOfType<PlayerProgress> ().areaProgress.TryGetValue (keyString, out temp)) {
                    FindObjectOfType<PlayerProgress> ().areaProgress.Add (keyString, true);
                    FoundSecretSFX ();
                }
                StartCoroutine (FadeTilemap (0f));
            }
        }
    }

    // Fade in/out tilemap
    private IEnumerator FadeTilemap (float targetAlpha) {
        if (lightToTurnOff != null) lightToTurnOff.SetActive (false);
        float alpha = tilemapToHide.color.a;
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            tilemapToHide.color = new Color (1, 1, 1, Mathf.Lerp (alpha, targetAlpha, t));
            yield return null;
        }
    }

    private void FoundSecretSFX () {
        AudioManager.Instance.Play ("Found_SecretArea");
    }
}