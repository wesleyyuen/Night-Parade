using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTriggerLoadLevel : MonoBehaviour {
    [SerializeField] private string levelToLoad = "";
    private void OnTriggerEnter2D (Collider2D collider) {
        if (collider.CompareTag ("Player")) {
            PlayerData playerVariables = new PlayerData(collider.gameObject, SceneManager.GetActiveScene ().buildIndex, SaveManager.GetLoadIndex());
            FindObjectOfType<GameMaster> ().RequestSceneChange (levelToLoad, playerVariables);
        }
    }
}