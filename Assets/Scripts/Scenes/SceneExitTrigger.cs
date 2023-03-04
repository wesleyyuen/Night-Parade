﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExitTrigger : MonoBehaviour
{
    [SerializeField] private string levelToLoad = "";

    // Load levelToLoad scene if triggered
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag ("Player")) {
            GameObject player = collider.gameObject;
            PlayerAnimations animations = player.GetComponent<PlayerAnimations>();
            PlayerMovement movement = player.GetComponent<PlayerMovement>();

            // Force Continue player's animation until off screen
            Utility.EnablePlayerControl(false);
            if (player.GetComponent<PlayerPlatformCollision>().onGround) {
                movement.MoveForwardForSeconds(2f);
            }
            
            Scene currentScene = SceneManager.GetActiveScene();

            // Save player states and variables for next scene
            PlayerData data = new PlayerData(collider.gameObject, false, currentScene.buildIndex, SaveManager.Instance.GetLoadIndex());
            
            // Play Scene Transition
            ISceneTransition transition = GetComponentInChildren<ISceneTransition>();
            if (transition != null)
                transition.StartSceneTransitionOut(levelToLoad, ref data);
            else
                Debug.LogError("Missing Scene Transition in " + currentScene.name + "!");
        }
    }
}

/*
    Following Version for all levels in one scene
*/
/*
using System.Collections;
using UnityEngine;
using Cinemachine;

public class SceneExitTrigger : MonoBehaviour
{
    [SerializeField] GameObject leftVCam;
    [SerializeField] GameObject rightVCam;
    [SerializeField] string levelToLoad = "";

    // Load levelToLoad scene if triggered
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) {
            GameObject player = collider.gameObject;
            PlayerAnimations animations = player.GetComponent<PlayerAnimations>();
            PlayerMovement movement = player.GetComponent<PlayerMovement>();

            Utility.FreezePlayer(1f);

            if (player.GetComponent<PlayerPlatformCollision>().onGround) {
                StartCoroutine(Utility.DoCoroutineAfterSeconds(1f, movement.MoveForwardForSeconds(0.4f))); 
            }

            // Enters from left to right
            if (collider.transform.position.x <= transform.position.x) {
                rightVCam.SetActive(true);
                rightVCam.GetComponent<CinemachineVirtualCameraBase>().MoveToTopOfPrioritySubqueue();
                leftVCam.SetActive(false);
            }
            // Enters from right to left
            else {
                leftVCam.SetActive(true);
                leftVCam.GetComponent<CinemachineVirtualCameraBase>().MoveToTopOfPrioritySubqueue();
                rightVCam.SetActive(false);
            }
        }
    }
}
*/