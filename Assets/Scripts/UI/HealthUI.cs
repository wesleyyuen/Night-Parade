using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    static HealthUI _instance;
    public static HealthUI Instance {
        get  {return _instance; }
    }
    PlayerHealth _playerHealth;
    [SerializeField] Material _flashMaterial;
    [SerializeField] Image[] hearts;
    // int[] _heartsSprite; // TODO: avoid direct sprite comparison
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite threeQuartersHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite quarterHeart;
    [SerializeField] Sprite emptyHeart;
    const float kFadeInDuration = 0.3f;
    
    void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }

        // _heartsSprite = new int[hearts.Length];

        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].gameObject.transform.localScale = Vector3.zero;
            Material mat = new Material(_flashMaterial); 
            mat.SetFloat("_FlashAmount", 0);
            mat.SetColor("_FlashColor", Color.white);
            hearts[i].material = mat;
        }
    }

    public void UpdatePlayerHealthReference(PlayerHealth reference)
    {
        _playerHealth = reference;
    }

    public void Intro()
    {
        FadeUI(true);
    }

    public void Outro()
    {
        StopAllCoroutines();
        FadeUI(false, true);
    }

    void FadeUI(bool fadeIn, bool isInstant = false)
    {
        Vector3 from = new Vector3(fadeIn ? 0f : 1f, fadeIn ? 0f : 1f, 1f);
        Vector3 to = new Vector3(fadeIn ? 1f : 0f, fadeIn ? 1f : 0f, 1f);

        for (int i = 0; i < hearts.Length; i++) {
            if (isInstant)
                hearts[i].gameObject.transform.localScale = to;
            else
                StartCoroutine(FadeUIHelper(hearts[i].gameObject, from, to, kFadeInDuration, kFadeInDuration * i));
        }
    }

    IEnumerator FadeUIHelper(GameObject obj, Vector3 from, Vector3 to, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Utility.ScaleGameObject(obj, from, to, duration));
    }

    public void UpdateHeartsUI(float duration = 0.75f)
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        if (_playerHealth == null) return;

        int health = _playerHealth.currHealth;
        int numOfFullHearts = health / 4;
        int maxHearts = _playerHealth.maxHealth / 4;

        if (duration > 0f)
            StartCoroutine(FlashHeart(true, duration));

        // Actually change sprites
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = (i < numOfFullHearts) ? fullHeart : emptyHeart;
                
            // disable hearts that exceed current maximum health
            hearts[i].enabled = i < maxHearts;
        }
        // Handle Reminder
        int reminder = health % 4;
        if (reminder != 0) {
            hearts[numOfFullHearts].sprite = (reminder == 3) ? threeQuartersHeart
                                           : (reminder == 2) ? halfHeart
                                           : quarterHeart;
        }

        if (duration > 0f)
            StartCoroutine(FlashHeart(false, duration));
    }

    IEnumerator FlashHeart(bool fadeIn, float duration)
    {
        float from = fadeIn ? 0f : 1f;
        float to = fadeIn ? 1f : 0f;
        int health = _playerHealth.currHealth;
        int numOfFullHearts = health / 4;
        int maxHearts = _playerHealth.maxHealth / 4;
        int reminder = health % 4;

        for (float t = 0f; t < 1f; t += Time.deltaTime / duration) {
            for (int i = 0; i < hearts.Length; i++) {
                if (i >= maxHearts) continue;

                // bool changed = i < numOfFullHearts ? hearts[i].sprite != fullHeart : hearts[i].sprite != emptyHeart;
                bool changed = false;
                if (i < numOfFullHearts)
                    changed = hearts[i].sprite != fullHeart;
                else if (i == numOfFullHearts && reminder != 0) {
                    changed = (reminder == 3) ? hearts[numOfFullHearts].sprite != threeQuartersHeart
                            : (reminder == 2) ? hearts[numOfFullHearts].sprite != halfHeart
                                            : hearts[numOfFullHearts].sprite != quarterHeart;
                } else
                    changed = hearts[i].sprite != emptyHeart;
                    
                if (changed || (i == numOfFullHearts)) {
                    hearts[i].material.SetFloat("_FlashAmount", Mathf.SmoothStep(from, to, t));
                }
                        
                // disable hearts that exceed current maximum health
                hearts[i].enabled = i < maxHearts;
            }
            yield return null;
        }
    }
}   