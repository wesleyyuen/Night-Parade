using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime = 1f;

    void Awake () {
        GetComponentInChildren<Canvas> ().enabled = true;
    }

    public void StartSceneTransition (string levelToLoad, PlayerData playerVariables) {
        StartCoroutine (SceneTransitionCoroutine (levelToLoad, playerVariables));
    }

    IEnumerator SceneTransitionCoroutine (string levelToLoad, PlayerData playerVariables) {
        animator.SetTrigger ("Start");
        yield return new WaitForSeconds (transitionTime);
        FindObjectOfType<GameMaster> ().RequestSceneChange (levelToLoad, playerVariables);
    }
}