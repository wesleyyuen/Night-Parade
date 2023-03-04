using System.Collections;
using UnityEngine;
using Zenject;
using TMPro;
using MEC;
using DG.Tweening;

public class MonUI : MonoBehaviour
{
    private EventManager _eventManager;
    [SerializeField] private TextMeshProUGUI monText;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private float showingDuration;
    [SerializeField] private float fadingDuration;

    [Inject]
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    private void OnEnable()
    {
        _eventManager.Event_OnPlayerMonIncreased += UpdateMonUI;
        _eventManager.Event_OnUIIntro += Intro;
        _eventManager.Event_OnUIOutro += Outro;
    }

    private void OnDisable()
    {
        _eventManager.Event_OnPlayerMonIncreased -= UpdateMonUI;
        _eventManager.Event_OnUIIntro -= Intro;
        _eventManager.Event_OnUIOutro -= Outro;
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

    public void UpdateMonUI(int prev, int curr)
    {
        SetText(prev);
        StartCoroutine(_ShowMonChangeCoroutine(curr));
    }

    private IEnumerator _ShowMonChangeCoroutine(int amount)
    {
        // Fade In
        canvas.DOFade(1f, fadingDuration);

        // Display current coins on hand as text
        yield return new WaitForSeconds(showingDuration * 0.2f);
        // Using LeanTween for TextMeshPro
        LeanTween.cancel(monText.gameObject);
        LeanTween.value(float.Parse(monText.text), (float)amount, 0.5f)
            .setOnUpdate(SetText)
            .setOnComplete(() => {
                monText.text = (amount).ToString();
            });
        yield return new WaitForSeconds(showingDuration * 0.8f);

        // Fade Out
        canvas.DOFade(0f, fadingDuration);
    }

    private void SetText(float value)
    {
        monText.text = ((int) value).ToString();
    }
}