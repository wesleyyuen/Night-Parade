using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonUI : MonoBehaviour
{
    static MonUI _instance;
    public static MonUI Instance {
        get  {return _instance; }
    }
    PlayerInventory playerInventory;
    [SerializeField] TextMeshProUGUI monText;
    [SerializeField] CanvasGroup canvas;

    [SerializeField] float showingDuration;
    [SerializeField] float fadingDuration;

    void Awake ()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
    }

    void Start()
    {
        canvas.alpha = 0;
    }

    public void ShowMonChange()
    {
        StartCoroutine(ShowMonChangeCoroutine());
    }

    IEnumerator ShowMonChangeCoroutine()
    {
        // Fade in gameObject
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingDuration * 2) {
            canvas.alpha = Mathf.Lerp (0f, 1f, t);
            yield return null;
        }

        // Display text
        UpdateMon();
        yield return new WaitForSeconds (showingDuration);

        // Fade out text
        for (float t = 0f; t < 1f; t += Time.deltaTime / fadingDuration) {
            canvas.alpha = Mathf.Lerp (1f, 0f, t);
            yield return null;
        }
    }

    void UpdateMon()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        if (playerInventory == null) return;

        // Display current coins on hand as text, space at the end to force spacing
        monText.text = playerInventory.coinOnHand.ToString() + " ";
    }
}