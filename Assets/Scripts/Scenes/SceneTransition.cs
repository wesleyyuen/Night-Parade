using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] protected float _transitionTime = 1f;

    public void StartSceneTransition(string levelToLoad, ref PlayerData playerVariables)
    {
        StartCoroutine(SceneTransitionCoroutine(levelToLoad, playerVariables));
    }

    protected virtual IEnumerator SceneTransitionCoroutine(string levelToLoad, PlayerData playerVariables)
    {
        GetComponentInChildren<Animator>().SetTrigger("Start");
        yield return new WaitForSecondsRealtime(_transitionTime);
        GameMaster.Instance.RequestSceneChange(levelToLoad, ref playerVariables);
    }
}