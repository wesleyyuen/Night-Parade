using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using MEC;
using DG.Tweening;

public class MonUI : MonoBehaviour
{
    PlayerInventory _playerInventory;
    [SerializeField] TextMeshProUGUI monText;
    [SerializeField] CanvasGroup canvas;
    [SerializeField] float showingDuration;
    [SerializeField] float fadingDuration;

    private void OnEnable()
    {
        GameMaster gm = FindObjectOfType<GameMaster>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        GameMaster.Event_UIIntro += Intro;
        GameMaster.Event_UIOutro += Outro;
    }

    private void OnDisable()
    {
        GameMaster gm = FindObjectOfType<GameMaster>();
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        GameMaster.Event_UIIntro -= Intro;
        GameMaster.Event_UIOutro -= Outro;
    }

    // Update Player References and add Observers
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        monText.text = SaveManager.Instance.savedPlayerData.CoinsOnHand.ToString() + " ";
        if (_playerInventory)
            _playerInventory.Event_MonChange += UpdateMonUI;
    }

    // Remove Observers
    private void OnSceneUnloaded(Scene scene)
    {
        if (_playerInventory)
            _playerInventory.Event_MonChange -= UpdateMonUI;
    }

    private void Intro()
    {
        canvas.alpha = 0f;
    }

    private void Outro()
    {
        Timing.PauseCoroutines();
        canvas.alpha = 0f;
    }

    public void UpdateMonUI()
    {
        StartCoroutine(_ShowMonChangeCoroutine());
    }

    private IEnumerator _ShowMonChangeCoroutine()
    {
        canvas.DOFade(1f, fadingDuration);

        // Display text
        yield return new WaitForSeconds (showingDuration * 0.2f);
        UpdateMonText();
        yield return new WaitForSeconds (showingDuration * 0.8f);

        canvas.DOFade(0f, fadingDuration);
    }

    private void UpdateMonText()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        if (_playerInventory == null) return;

        // Display current coins on hand as text, space at the end to force spacing
        // monText.text = _playerInventory.coinOnHand.ToString() + " ";
        LeanTween.cancel(monText.gameObject);
        LeanTween.value(float.Parse(monText.text), (float)_playerInventory.MonOnHand, 0.5f)
            .setOnUpdate(SetText)
            .setOnComplete(() => {
                monText.text = ((int)_playerInventory.MonOnHand).ToString();
            });
    }

    private void SetText(float value)
    {
        monText.text = ((int) value).ToString();
    }
}