using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CrossfadeTransition : MonoBehaviour, ISceneTransition
{
    [SerializeField] private float _transitionDuration;
    [SerializeField] private CanvasGroup _canvasGroup;
    private Tween _tween;
    float ISceneTransition.TransitionDuration
    {
        get => _transitionDuration;
    }

    private void Awake()
    {
        _canvasGroup.alpha = 1f;
    }
    
    public void StartSceneTransitionIn()
    {
        if (_tween != null) _tween.Kill(false);

        _canvasGroup.alpha = 1f;
        _tween = _canvasGroup.DOFade(0f, _transitionDuration)
                        .SetEase(Ease.InOutExpo)
                        .OnComplete(() => {
                            _canvasGroup.blocksRaycasts = false;
                            _canvasGroup.interactable = false;
                        });
    }

    public void StartSceneTransitionOut(string levelToLoad, ref PlayerData playerVariables)
    {
        if (_tween != null) _tween.Kill(false);

        PlayerData local = playerVariables;
        _canvasGroup.alpha = 0f;
        _tween = _canvasGroup.DOFade(1f, _transitionDuration)
                             .SetEase(Ease.InOutExpo)
                             .OnComplete(() => {
                                _canvasGroup.blocksRaycasts = false;
                                _canvasGroup.interactable = false;
                                GameMaster.Instance.RequestSceneChange(levelToLoad, ref local);
                             });
    }
}