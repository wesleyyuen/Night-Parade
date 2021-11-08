using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RevealHiddenArea : MonoBehaviour
{
    [SerializeField] string keyString;
    [SerializeField] GameObject vcam;
    [SerializeField] Tilemap tilemapToHide;
    [SerializeField] GameObject lightToTurnOff;
    [SerializeField] float alphaToFadeTo;
    [SerializeField] float fadingTime;
    [SerializeField] bool hideFromLeft;
    bool _hasVisited;

    void Start()
    {
        // Make hidden area semi-transparant when discovered before
        _hasVisited = SaveManager.Instance.HasScenePermaProgress(GameMaster.Instance.currentScene, keyString);
        if (_hasVisited) {
            tilemapToHide.color = new Color (1, 1, 1, alphaToFadeTo);
            Destroy(lightToTurnOff);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            // Find direction player enter the trigger from
            Vector3 dir = other.transform.position - gameObject.transform.position;
            if (hideFromLeft && dir.x < 0 || !hideFromLeft && dir.x > 0) {
                Utility.FadeGameObjectRecursively(tilemapToHide.gameObject, tilemapToHide.color.a, alphaToFadeTo, fadingTime);
                vcam.SetActive(false);
            } else {
                // Only play sound effect if First time walking into hidden area
                if (!_hasVisited) {
                    SaveManager.Instance.AddScenePermaProgress(GameMaster.Instance.currentScene, keyString, 1);
                    FoundSecretSFX();
                }
                Utility.FadeGameObjectRecursively(tilemapToHide.gameObject, tilemapToHide.color.a, 0f, fadingTime);
                vcam.SetActive(true);
            }
        }
    }

    void FoundSecretSFX()
    {
        // SoundManager.Instance.Play ("Found_SecretArea");
    }
}