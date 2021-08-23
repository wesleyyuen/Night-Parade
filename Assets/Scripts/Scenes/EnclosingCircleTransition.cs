using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnclosingCircleTransition : SceneTransition
{
    protected override IEnumerator SceneTransitionCoroutine(string levelToLoad, PlayerData playerVariables)
    {
        SpriteMask mask = GetComponentInChildren<SpriteMask>();

        for (float t = 0f; t < 1f; t += Time.deltaTime / _transitionTime) {
            // mask.
            // _mask.color = new Color (text.color.r, text.color.g, text.color.b, Mathf.SmoothStep (from, to, t));
            yield return null;
        }

        // yield return new WaitForSecondsRealtime(transitionTime);
        GameMaster.Instance.RequestSceneChange(levelToLoad, ref playerVariables);
    }
}