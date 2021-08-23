using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    static HealthUI _instance;
    public static HealthUI Instance {
        get  {return _instance; }
    }
    PlayerHealth _playerHealth;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite threeQuartersHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite quarterHeart;
    [SerializeField] Sprite emptyHeart;
    const float kFadeInDuration = 0.85f;
    const float kFadeInOffsetDuration = 0.3f;
    
    void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad (gameObject);
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
        for (int i = 0; i < hearts.Length; i++) {
            if (isInstant) {
                hearts[i].color = new Color(1f, 1f, 1f, to);
            } else {
                hearts[i].color = new Color(1f, 1f, 1f, from);
                StartCoroutine(Utility.FadeImage(hearts[i], from, to, kFadeInDuration + kFadeInOffsetDuration * i));
            }
        }
    }

    public void UpdateHeartsUI()
    {
        _playerHealth = FindObjectOfType<PlayerHealth> ();
        if (_playerHealth == null) return;

        int numOfFullHearts = _playerHealth.currHealth / 4;
        int maxHearts = _playerHealth.maxHealth / 4;

        // Display hearts
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = (i < numOfFullHearts) ? fullHeart : emptyHeart;
                
            // disable hearts that exceed current maximum health
            hearts[i].enabled = i < maxHearts;
        }

        // Handle Reminder
        int reminder = _playerHealth.currHealth % 4;
        if (reminder != 0) {
            hearts[numOfFullHearts].sprite = (reminder == 3) ? threeQuartersHeart
                                                    : (reminder == 2) ? halfHeart : quarterHeart;
        }
    }
}