using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnclosingCircleTransition : ISceneTransition
{
    [SerializeField] private float _transitionDuration;
    // [SerializeField] private CanvasGroup _canvasGroup;
    float ISceneTransition.TransitionDuration
    {
        get => _transitionDuration;
    }
    // [SerializeField] private GameObject _mask;
    // [SerializeField] private RectTransform _backer;
    // [SerializeField] private Vector2 _maskPosition;
    // private Transform _player;
    // private Image _maskImage;
    // private RectTransform _rect;
    // private int _width = 1600;
    // private int _height = 900;

    // protected override void Awake()
    // {
    //     base.Awake();

    //     _maskImage = _mask.GetComponent<Image>();
    //     _rect = _mask.GetComponent<RectTransform>();
    //     _player = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<Transform>();

    //     _backer.sizeDelta = new Vector2(_width * 2, _height * 2);

    //     // Diameter = Diagonal of Screen
    //     float diameter = Mathf.Sqrt(Mathf.Pow(_width, 2) + Mathf.Pow(_height, 2));
    //     _rect.sizeDelta = new Vector2(diameter, diameter);

    //     _rect.anchoredPosition = _maskPosition;
    // }

    // protected override IEnumerator _SceneTransitionInCoroutine()
    // {        
    //     // Animator auto plays animation
    //     for (float t = 0f; t < 1f; t += Time.deltaTime / _transitionTime) {
    //         _rect.anchoredPosition = new Vector2(Mathf.SmoothStep(_maskPosition.x, 0f, t), Mathf.SmoothStep(_maskPosition.y, 0f, t));
    //         yield return null;
    //     }
    // }

    // protected override IEnumerator _SceneTransitionOutCoroutine(string levelToLoad, PlayerData playerVariables)
    // {
    //     _animator.SetTrigger("Start");

    //     for (float t = 0f; t < 1f; t += Time.deltaTime / _transitionTime) {
    //         _rect.anchoredPosition = new Vector2(Mathf.SmoothStep(0f, _maskPosition.x, t), Mathf.SmoothStep(0f, _maskPosition.y, t));
    //     }
    //     yield return new WaitForSecondsRealtime(_transitionTime);
    //     GameMaster.Instance.RequestSceneChange(levelToLoad, ref playerVariables);
    // }

    public void StartSceneTransitionIn()
    {
    }

    public void StartSceneTransitionOut(string levelToLoad, ref PlayerData playerVariables)
    {
    }
}
