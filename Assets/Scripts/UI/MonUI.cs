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

    void Awake ()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
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
        Utility.SetAlphaRecursively(canvas, 0f);
    }

    public void ShowMonChange()
    {
        StartCoroutine(ShowMonChangeCoroutine());
    }

    public void FixCoroutineDeath()
    {
        Utility.SetAlphaRecursively(canvas, 0f);
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