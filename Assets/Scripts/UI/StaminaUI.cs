using UnityEngine;
using UnityEngine.UI;
using MEC;
using Zenject;
using DG.Tweening;

public class StaminaUI : MonoBehaviour
{
    private EventManager _eventManager;
    [SerializeField] CanvasGroup UIObject;
    [SerializeField] Image barFill;
    const float kFadeDuration = 0.85f;

    [Inject]
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    private void Awake()
    {
        UIObject.alpha = 0f;
    }

    private void OnEnable()
    {
        // GameMaster.Instance.Event_UIIntro += Intro;
        // GameMaster.Instance.Event_UIOutro += Outro;
                _eventManager.Event_OnUIIntro += Intro;
        _eventManager.Event_OnUIOutro += Outro;
    }

    private void OnDisable()
    {
                _eventManager.Event_OnUIIntro -= Intro;
        _eventManager.Event_OnUIOutro -= Outro;
        // GameMaster.Instance.Event_UIIntro -= Intro;
        // GameMaster.Instance.Event_UIOutro -= Outro;
    }

    private void Intro()
    {
        FadeUI(true);
    }

    private void Outro()
    {
        FadeUI(false, true);
    }

    private void FadeUI(bool fadeIn, bool isInstant = false)
    {
        StopAllCoroutines();

        float from = fadeIn ? 0f : 1f;
        float to = fadeIn ? 1f : 0f;
    
        if (isInstant)
            UIObject.alpha = to;
        else {
            UIObject.alpha = from;
            UIObject.DOFade(to, kFadeDuration);
        }
    }

    public void UpdateStaminaUI(float percent, bool isInit = false)
    {
        barFill.fillAmount = percent;
    }
}
