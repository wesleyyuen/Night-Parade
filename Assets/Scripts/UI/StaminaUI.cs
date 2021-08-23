using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    static StaminaUI _instance;
    public static StaminaUI Instance {
        get  {return _instance; }
    }
    [SerializeField] Image barFrame;
    [SerializeField] Image barBacker;
    [SerializeField] Image staminaBar;
    float kFadeDuration = 0.85f;

    void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy (gameObject);
        }
    }

    public void Intro()
    {
        FadeUI(true);
    }

    public void Outro()
    {
        FadeUI(false, true);
    }


    private void FadeUI(bool fadeIn, bool isInstant = false)
    {
        StopAllCoroutines();

        float from = fadeIn ? 0f : 1f;
        float to = fadeIn ? 1f : 0f;
        if (isInstant) {
            barFrame.color = new Color(1f, 1f, 1f, to);
            barBacker.color = new Color(barBacker.color.r, barBacker.color.g, barBacker.color.b, to);
            staminaBar.color  = new Color(staminaBar.color.r, staminaBar.color.g, staminaBar.color.b, to);
        } else {
            barFrame.color = new Color(1f, 1f, 1f, from);
            barBacker.color = new Color(barBacker.color.r, barBacker.color.g, barBacker.color.b, from);
            staminaBar.color  = new Color(staminaBar.color.r, staminaBar.color.g, staminaBar.color.b, from);
            StartCoroutine(Utility.FadeImage(barFrame, from, to, kFadeDuration));
            StartCoroutine(Utility.FadeImage(barBacker, from, to, kFadeDuration));
            StartCoroutine(Utility.FadeImage(staminaBar, from, to, kFadeDuration));
        }
    }

    public void UpdateStaminaUI(float percent, bool isInit = false)
    {
        staminaBar.fillAmount = percent;
    }
}
