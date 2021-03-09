using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonUI : MonoBehaviour {

    private static MonUI Instance;
    private PlayerInventory playerInventory;
    [SerializeField] private TextMeshProUGUI monText;
    [SerializeField] private CanvasGroup canvas;

    [SerializeField] private float showingDuration;
    [SerializeField] private float fadingDuration;

    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
    }

    void Start () {
        playerInventory = FindObjectOfType<PlayerInventory> ();
        if (playerInventory)
            monText.text = playerInventory.coinOnHand.ToString ();
    }

    public void UpdateMon() {
        playerInventory = FindObjectOfType<PlayerInventory> ();
        if (playerInventory == null) return;

        // Display current coins on hand as text
        monText.text = playerInventory.coinOnHand.ToString ();
    }

    // private IEnumerator showMonChange () {
    //     canvas.gameObject.SetActive(true);
    //     // Fade in gameObject
    //     for (float t = 0f; t < 1f; t += Time.deltaTime / fadingDuration) {
    //         canvas.alpha = Mathf.Lerp (0f, 1f, t);
    //         yield return null;
    //     }
    //     // Display text
    //     yield return new WaitForSeconds (showingDuration);
    //     // Fade out text
    //     for (float t = 0f; t < 1f; t += Time.deltaTime / fadingDuration) {
    //         canvas.alpha = Mathf.Lerp (1f, 0f, t);
    //         yield return null;
    //     }
    //     canvas.gameObject.SetActive(false);
    // }
}