using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using MEC;

public class MonUI : MonoBehaviour
{
    PlayerInventory _playerInventory;
    [SerializeField] TextMeshProUGUI monText;
    [SerializeField] GameObject canvas;

    [SerializeField] float showingDuration;
    [SerializeField] float fadingDuration;

    void OnEnable()
    {
        GameMaster gm = FindObjectOfType<GameMaster>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        gm.Event_UIIntro += Intro;
        gm.Event_UIOutro += Outro;
    }

    void OnDisable()
    {
        GameMaster gm = FindObjectOfType<GameMaster>();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        gm.Event_UIIntro -= Intro;
        gm.Event_UIOutro -= Outro;
    }

    // Update Player References and add Observers
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        if (_playerInventory)
            _playerInventory.Event_MonChange += UpdateMonUI;
    }

    // Remove Observers
    void OnSceneUnloaded(Scene scene)
    {
        if (_playerInventory)
            _playerInventory.Event_MonChange -= UpdateMonUI;
    }

    void Intro()
    {
        Utility.SetAlphaRecursively(canvas, 0f);
    }

    void Outro()
    {
        Timing.PauseCoroutines();
        Utility.SetAlphaRecursively(canvas, 0f);
    }

    public void UpdateMonUI()
    {
        // Set Text Once from save data
        monText.text = SaveManager.Instance.savedPlayerData.CoinsOnHand.ToString() + " ";
        StartCoroutine(_ShowMonChangeCoroutine());
    }

    IEnumerator _ShowMonChangeCoroutine()
    {
        Utility.FadeGameObjectRecursively(canvas, 0f, 1f, fadingDuration);

        // Display text
        yield return new WaitForSeconds (showingDuration * 0.2f);
        UpdateMonText();
        yield return new WaitForSeconds (showingDuration * 0.8f);

        Utility.FadeGameObjectRecursively(canvas, 1f, 0f, fadingDuration);
    }

    void UpdateMonText()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        if (_playerInventory == null) return;

        // Display current coins on hand as text, space at the end to force spacing
        monText.text = _playerInventory.coinOnHand.ToString() + " ";
    }
}