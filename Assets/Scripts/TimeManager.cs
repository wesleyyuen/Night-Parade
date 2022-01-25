using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;
    public static TimeManager Instance {
        get  {return _instance; }
    }
    private const float ORIGINAL_FIXED_DELTA_TIME = 0.02f;
    private IEnumerator _slowMoCoroutine;

    private void Awake()
    {
        _instance = this;
    }

    public void SetTimeScale(float scale, float duration = 0f)
    {
        if (duration == 0f) {
            Time.timeScale = scale;
            Time.fixedDeltaTime = ORIGINAL_FIXED_DELTA_TIME * scale;
        } else {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, scale, duration);
            DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, ORIGINAL_FIXED_DELTA_TIME * scale, duration);
        }
    }

    public void SlowTimeForSeconds(float scale, float delay, bool isEaseOut = false)
    {
        if (_slowMoCoroutine != null)
            StopCoroutine(_slowMoCoroutine);
        
        _slowMoCoroutine = _SlowTimeForSeconds(scale, delay, isEaseOut);
        StartCoroutine(_slowMoCoroutine);
    }
    

    private IEnumerator _SlowTimeForSeconds(float scale, float delay, bool isEaseOut)
    {
        SetTimeScale(scale);

        yield return new WaitForSecondsRealtime(delay);

        if (!PauseMenu.isPuased) {
            if (isEaseOut) {
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 1f);
            } else {
                Time.timeScale = 1f;
            }
        }

        if (isEaseOut) {
            DOTween.To(() => Time.fixedDeltaTime, x => Time.fixedDeltaTime = x, ORIGINAL_FIXED_DELTA_TIME, 1f);
        } else {
            Time.fixedDeltaTime = ORIGINAL_FIXED_DELTA_TIME;
        }
    }
}
