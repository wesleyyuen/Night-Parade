using UnityEngine;
using UnityEngine.UI;
using MEC;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] GameObject UIObject;
    [SerializeField] Image barFill;
    const float kFadeDuration = 0.85f;

    void Awake()
    {
        Utility.SetAlphaRecursively(UIObject, 0f);
    }

    void OnEnable()
    {
        GameMaster gm = FindObjectOfType<GameMaster>();
        gm.Event_UIIntro += Intro;
        gm.Event_UIOutro += Outro;
    }

    void OnDisable()
    {
        GameMaster gm = FindObjectOfType<GameMaster>();
        gm.Event_UIIntro -= Intro;
        gm.Event_UIOutro -= Outro;
    }

    void Intro()
    {
        FadeUI(true);
    }

    void Outro()
    {
        FadeUI(false, true);
    }

    void FadeUI(bool fadeIn, bool isInstant = false)
    {
        StopAllCoroutines();

        float from = fadeIn ? 0f : 1f;
        float to = fadeIn ? 1f : 0f;

        // Utility.FadeGameObjectRecursively(UIObject, from, to, isInstant ? 0f : kFadeDuration);
        if (isInstant)
            Utility.SetAlphaRecursively(UIObject, to);
        else
            LocalRecursiveFade(UIObject, from, to, isInstant);
    }

    // TODO: For some reason, Utility.FadeGameObjectRecursively doesn't work
    void LocalRecursiveFade(GameObject go, float from, float to, bool isInstant)
    {
        foreach (Transform child in go.transform) {
            StartCoroutine(Utility.FadeGameObject(child.gameObject, from, to, isInstant ? 0f : kFadeDuration));
            
            if (child.childCount > 0)
                LocalRecursiveFade(child.gameObject, from, to, isInstant);
        }
    }

    public void UpdateStaminaUI(float percent, bool isInit = false)
    {
        barFill.fillAmount = percent;
    }
}
