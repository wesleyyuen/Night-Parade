using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScript : MonoBehaviour
{
    [SerializeField] protected Transform _playerGroup;
    [SerializeField] protected SceneTransition _transition;
    
    protected virtual void Start()
    {
        _transition.StartSceneTransitionIn();
    }
}
