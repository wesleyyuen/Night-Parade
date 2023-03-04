using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MEC;
using DG.Tweening;
using Zenject;

public class HealthUI : MonoBehaviour
{
    private EventManager _eventManager;
    [SerializeField] Material _flashMaterial;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite threeQuartersHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite quarterHeart;
    [SerializeField] Sprite emptyHeart;

    [Inject]
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
    }
    
    private void Awake()
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

    private void OnEnable()
    {
        _eventManager.Event_OnPlayerDamaged += UpdateHeartsUI;
        _eventManager.Event_OnUIIntro += Intro;
        _eventManager.Event_OnUIOutro += Outro;
    }

    private void OnDisable()
    {
        _eventManager.Event_OnPlayerDamaged -= UpdateHeartsUI;
        _eventManager.Event_OnUIIntro -= Intro;
        _eventManager.Event_OnUIOutro -= Outro;
    }

    private void Intro()
    {
        FadeUI(true);
    }

    private void Outro()
    {
        // StopAllCoroutines();
        DOTween.PauseAll();
        FadeUI(false, true);
    }

    private void FadeUI(bool fadeIn, bool isInstant = false)
    {
        Vector3 from = new Vector3(fadeIn ? 0f : 1f, fadeIn ? 0f : 1f, 1f);
        Vector3 to = new Vector3(fadeIn ? 1f : 0f, fadeIn ? 1f : 0f, 1f);

        for (int i = 0; i < hearts.Length; i++) {
            if (isInstant)
                hearts[i].gameObject.transform.localScale = to;
            else {
                hearts[i].gameObject.transform.localScale = from;
                hearts[i].rectTransform.DOScale(to, 0.3f).SetDelay(0.25f * i);
            }
        }
    }

    public void UpdateHeartsUI(int prevHealth, int currHealth, int maxHealth, float duration)
    {
        if (currHealth <= 0) return;

        int numOfFullHearts = currHealth / 4;
        int maxHearts = maxHealth / 4;

        if (duration > 0f) {
            Timing.RunCoroutine(_FlashHeart(true, duration, prevHealth, currHealth, maxHealth).CancelWith(gameObject));
        }

        // Actually change sprites
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = (i < numOfFullHearts) ? fullHeart : emptyHeart;
                
            // disable hearts that exceed current maximum health
            hearts[i].enabled = i < maxHearts;
        }
        // Handle Reminder
        int reminder = currHealth % 4;
        if (reminder != 0) {
            hearts[numOfFullHearts].sprite = (reminder == 3) ? threeQuartersHeart
                                           : (reminder == 2) ? halfHeart
                                           : quarterHeart;
        }

        if (duration > 0f) {
            Timing.RunCoroutine(_FlashHeart(false, duration, prevHealth, currHealth, maxHealth).CancelWith(gameObject));
        }
    }

    IEnumerator<float> _FlashHeart(bool fadeIn, float duration, int prevHealth, int currHealth, int maxHealth)
    {
        float from = fadeIn ? 0f : 1f;
        float to = fadeIn ? 1f : 0f;
        int numOfFullHearts = currHealth / 4;
        int maxHearts = maxHealth / 4;
        int reminder = currHealth % 4;

        for (float t = 0f; t < 1f; t += Timing.DeltaTime / duration) {
            for (int i = 0; i < hearts.Length; i++) {
                if (i >= maxHearts) continue;
                    
                if (ShouldHeartBeUpdatedAtIndex(i, prevHealth, currHealth)) {
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