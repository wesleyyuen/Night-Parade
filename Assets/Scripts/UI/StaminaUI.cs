using UnityEngine;
using UnityEngine.UI;
using MEC;
using DG.Tweening;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] CanvasGroup UIObject;
    [SerializeField] Image barFill;
    const float kFadeDuration = 0.85f;

    private void Awake()
    {
        UIObject.alpha = 0f;
    }

    private void OnEnable()
    {
        GameMaster gm = FindObjectOfType<GameMaster>();
        GameMaster.Event_UIIntro += Intro;
        GameMaster.Event_UIOutro += Outro;
    }

    private void OnDisable()
    {
        GameMaster gm = FindObjectOfType<GameMaster>();
        GameMaster.Event_UIIntro -= Intro;
        GameMaster.Event_UIOutro -= Outro;
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
