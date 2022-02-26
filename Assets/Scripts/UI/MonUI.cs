using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
// using Zenject;
using TMPro;
using MEC;
using DG.Tweening;

public class MonUI : MonoBehaviour
{
    private PlayerInventory _playerInventory;
    [SerializeField] private TextMeshProUGUI monText;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private float showingDuration;
    [SerializeField] private float fadingDuration;

    // [Inject]

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        if (GameMaster.Instance != null) {
            GameMaster.Instance.Event_UIIntro += Intro;
            GameMaster.Instance.Event_UIOutro += Outro;
        } else {
            Intro();
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        GameMaster.Instance.Event_UIIntro -= Intro;
        GameMaster.Instance.Event_UIOutro -= Outro;
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
        yield return new WaitForSeconds(showingDuration * 0.2f);
        UpdateMonText();
        yield return new WaitForSeconds(showingDuration * 0.8f);

        canvas.DOFade(0f, fadingDuration);
    }

    private void UpdateMonText()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        if (_playerInventory == null) return;

        // Display current coins on hand as text
        // Using LeanTween for TextMeshPro
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