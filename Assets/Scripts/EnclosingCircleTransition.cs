using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnclosingCircleTransition : SceneTransition
{
    [SerializeField] GameObject _mask;
    [SerializeField] RectTransform _backer;
    [SerializeField] Vector2 _maskPosition;
    Transform _player;
    Image _maskImage;
    RectTransform _rect;
    int _width = 1600;
    int _height = 900;

    protected override void Awake()
    {
        base.Awake();

        _maskImage = _mask.GetComponent<Image>();
        _rect = _mask.GetComponent<RectTransform>();
        _player = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<Transform>();

        _backer.sizeDelta = new Vector2(_width * 2, _height * 2);

        // Diameter = Diagonal of Screen
        float diameter = Mathf.Sqrt(Mathf.Pow(_width, 2) + Mathf.Pow(_height, 2));
        _rect.sizeDelta = new Vector2(diameter, diameter);

        _rect.anchoredPosition = _maskPosition;
    }

    protected override IEnumerator SceneTransitionInCoroutine()
    {        
        // Animator auto plays animation
        for (float t = 0f; t < 1f; t += Time.deltaTime / _transitionTime) {
            _rect.anchoredPosition = new Vector2(Mathf.SmoothStep(_maskPosition.x, 0f, t), Mathf.SmoothStep(_maskPosition.y, 0f, t));
            yield return null;
        }
    }

    protected override IEnumerator SceneTransitionOutCoroutine(string levelToLoad, PlayerData playerVariables)
    {
        _animator.SetTrigger("Start");

        for (float t = 0f; t < 1f; t += Time.deltaTime / _transitionTime) {
            _rect.anchoredPosition = new Vector2(Mathf.SmoothStep(0f, _maskPosition.x, t), Mathf.SmoothStep(0f, _maskPosition.y, t));
        }
        yield return new WaitForSecondsRealtime(_transitionTime);
        GameMaster.Instance.RequestSceneChange(levelToLoad, ref playerVariables);
    }
}
