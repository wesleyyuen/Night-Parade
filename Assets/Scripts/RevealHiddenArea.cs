using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RevealHiddenArea : MonoBehaviour {

    public Tilemap tilemapToHide;
    public GameObject lightToTurnOff;
    public float fadingTime;
    public bool hideFromLeft;
    bool coroutining;

    void OnTriggerExit2D (Collider2D other) {
        if (other.CompareTag ("Player")) {
            Vector3 dir = other.transform.position - gameObject.transform.position;
            if (hideFromLeft && dir.x < 0 || !hideFromLeft && dir.x > 0) {
                StartCoroutine (FadeTilemap (0.7f));
            } else {
                // TODO: change to use flag, and save if player discover this area, as well as if play collected the coins
                if (tilemapToHide.color.a == 1f) {
                    FoundSecretSFX ();
                }
                StartCoroutine (FadeTilemap (0f));
            }
        }
    }

    IEnumerator FadeTilemap (float alphaToFadeTo) {
        coroutining = true;
        lightToTurnOff.SetActive (false);
        float alpha = tilemapToHide.color.a;
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingTime) {
            tilemapToHide.color = new Color (1, 1, 1, Mathf.Lerp (alpha, alphaToFadeTo, t));
            yield return null;
        }
        coroutining = false;
    }

    void FoundSecretSFX () {
        FindObjectOfType<AudioManager> ().Play ("Found_SecretArea");
    }
}