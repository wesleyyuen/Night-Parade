using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaScript : MonoBehaviour
{
    [SerializeField] protected Transform _playerGroup;
    [SerializeField] protected SceneTransition _transition;
    protected virtual void Start()
    {
        GameMaster.Instance.UpdateCurrentScene();

        _transition.StartSceneTransitionIn();

        // TODO: somehow continue monUI coroutine instead
        MonUI.Instance.FixCoroutineDeath();
    }
}
