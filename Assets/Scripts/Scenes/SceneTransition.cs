using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] protected float _transitionTime = 1f;
    protected Animator _animator;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _animator.speed = 1f/_transitionTime;
    }

    public void StartSceneTransitionIn()
    {
        StartCoroutine(SceneTransitionInCoroutine());
    }

    public void StartSceneTransitionOut( string levelToLoad, ref PlayerData playerVariables)
    {
        StartCoroutine(SceneTransitionOutCoroutine(levelToLoad, playerVariables));
    }

    protected virtual IEnumerator SceneTransitionInCoroutine()
    {
        yield return new WaitForSecondsRealtime(_transitionTime);
    }

    protected virtual IEnumerator SceneTransitionOutCoroutine(string levelToLoad, PlayerData playerVariables)
    {
        _animator.speed = 1f/_transitionTime;
        _animator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(_transitionTime);
        GameMaster.Instance.RequestSceneChange(levelToLoad, ref playerVariables);
    }
}