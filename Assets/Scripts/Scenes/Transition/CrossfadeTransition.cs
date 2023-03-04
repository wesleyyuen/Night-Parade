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

    public void StartSceneTransitionIn()
    {
        if (_tween != null) _tween.Kill(true);

        _canvasGroup.alpha = 1f;
        _tween = _canvasGroup.DOFade(0f, _transitionDuration)
                        .SetEase(Ease.InCubic)
                        .OnComplete(() => {
                            _canvasGroup.blocksRaycasts = false;
                            _canvasGroup.interactable = false;
                        });
    }

    public void StartSceneTransitionOut(string levelToLoad, ref PlayerData playerData)
    {
        if (_tween != null) _tween.Kill(true);

        PlayerData player = playerData;
        _canvasGroup.alpha = 0f;
        _tween = _canvasGroup.DOFade(1f, _transitionDuration)
                             .SetEase(Ease.InCubic)
                             .OnComplete(() => {
                                _canvasGroup.blocksRaycasts = false;
                                _canvasGroup.interactable = false;
                                GameMaster.Instance.RequestSceneChange(levelToLoad, ref player);
                             });
    }
}