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
    [SerializeField] GameObject canvas;

    [SerializeField] float showingDuration;
    [SerializeField] float fadingDuration;

    void Awake()
    {
        if (_instance == null) {
            _instance = this;
            // DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    public void Intro()
    {
        Utility.SetAlphaRecursively(canvas, 0f);
    }

    public void Outro()
    {
        StopAllCoroutines();
        Utility.SetAlphaRecursively(canvas, 0f);
    }

    public void ShowMonChange()
    {
        // Set Text Once from save data
        monText.text = SaveManager.Instance.savedPlayerData.CoinsOnHand.ToString() + " ";
        StartCoroutine(ShowMonChangeCoroutine());
    }

    IEnumerator ShowMonChangeCoroutine()
    {
        Utility.FadeGameObjectRecursively(canvas, 0f, 1f, fadingDuration);

        // Display text
        yield return new WaitForSeconds (showingDuration * 0.2f);
        UpdateMon();
        yield return new WaitForSeconds (showingDuration * 0.8f);

        Utility.FadeGameObjectRecursively(canvas, 1f, 0f, fadingDuration);
    }

    void UpdateMon()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        if (playerInventory == null) return;

        // Display current coins on hand as text, space at the end to force spacing
        monText.text = playerInventory.coinOnHand.ToString() + " ";
    }
}