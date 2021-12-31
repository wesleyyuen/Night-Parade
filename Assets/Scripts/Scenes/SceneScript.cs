using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScript : MonoBehaviour
{
    [SerializeField] protected Transform _playerGroup;
    [SerializeField] protected GameObject _transitionGO;
    protected ISceneTransition _transition;

    protected virtual void Start()
    {
        _transition = _transitionGO.GetComponent<ISceneTransition>();
        _transition.StartSceneTransitionIn();
    }
}
