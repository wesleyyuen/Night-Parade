using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MEC;

public class HealthUI : MonoBehaviour
{
    PlayerHealth _playerHealth;
    [SerializeField] Material _flashMaterial;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite threeQuartersHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite quarterHeart;
    [SerializeField] Sprite emptyHeart;
    const float kFadeInDuration = 0.3f;
    
    void Awake()
    {
        // Set Material
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].gameObject.transform.localScale = Vector3.zero;
            Material mat = new Material(_flashMaterial); 
            mat.SetFloat("_FlashAmount", 0);
            mat.SetColor("_FlashColor", Color.white);
            hearts[i].material = mat;
        }
    }

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
        _playerHealth = FindObjectOfType<PlayerHealth>();
        if (_playerHealth)
            _playerHealth.Event_HealthChange += UpdateHeartsUI;
    }

    // Remove Observers
    void OnSceneUnloaded(Scene scene)
    {
        if (_playerHealth)
            _playerHealth.Event_HealthChange -= UpdateHeartsUI;
    }

    void Intro()
    {
        FadeUI(true);
    }

    void Outro()
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
                StartCoroutine(_FadeUIHelper(hearts[i].gameObject, from, to, kFadeInDuration, kFadeInDuration * i));
        }
    }

    IEnumerator _FadeUIHelper(GameObject obj, Vector3 from, Vector3 to, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Utility.ScaleGameObject(obj, from, to, duration));
    }

    public void UpdateHeartsUI(int prevHealth, float duration = 0.75f)
    {
        int health = _playerHealth.currHealth;
        int numOfFullHearts = health / 4;
        int maxHearts = _playerHealth.maxHealth / 4;

        if (duration > 0f)
            Timing.RunCoroutine(_FlashHeart(true, duration, prevHealth).CancelWith(gameObject));

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
            Timing.RunCoroutine(_FlashHeart(false, duration, prevHealth).CancelWith(gameObject));
    }

    IEnumerator<float> _FlashHeart(bool fadeIn, float duration, int prevHealth)
    {
        float from = fadeIn ? 0f : 1f;
        float to = fadeIn ? 1f : 0f;
        int health = _playerHealth.currHealth;
        int numOfFullHearts = health / 4;
        int maxHearts = _playerHealth.maxHealth / 4;
        int reminder = health % 4;

        for (float t = 0f; t < 1f; t += Timing.DeltaTime / duration) {
            for (int i = 0; i < hearts.Length; i++) {
                if (i >= maxHearts) continue;
                    
                if (ShouldHeartBeUpdatedAtIndex(i, prevHealth, health)) {
                    hearts[i].material.SetFloat("_FlashAmount", Mathf.SmoothStep(from, to, t));
                }
            }
            yield return Timing.WaitForOneFrame;
        }
    }

    bool ShouldHeartBeUpdatedAtIndex(int index, int prevHealth, int currHealth)
    {
        int prevNumOfFullHearts = prevHealth / 4;
        int currNumOfFullHearts = currHealth / 4;
        
        int prevHeartAtIndex = index < prevNumOfFullHearts ? 4 : 0;
        int currHeartAtIndex = index < currNumOfFullHearts ? 4 : 0;

        int prevRemainder = prevHealth % 4;
        int currRemainder = currHealth % 4;
        if (index == prevNumOfFullHearts && prevRemainder != 0) prevHeartAtIndex = prevRemainder;
        if (index == currNumOfFullHearts && currRemainder != 0) currHeartAtIndex = currRemainder;

        return prevHeartAtIndex != currHeartAtIndex;
    }
}   