using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTriggerLoadScene : MonoBehaviour {
    [SerializeField] private string levelToLoad = "";

    // Load levelToLoad scene if triggered
    private void OnTriggerEnter2D (Collider2D collider) {
        if (collider.CompareTag ("Player")) {
            // Save player states and variables for next scene
            PlayerData playerVariables = new PlayerData(collider.gameObject, false, SceneManager.GetActiveScene ().buildIndex, SaveManager.GetLoadIndex());
            
            // Play Scene Transition
            FindObjectOfType<SceneTransition>().StartSceneTransition(levelToLoad, playerVariables);
        }
    }
}