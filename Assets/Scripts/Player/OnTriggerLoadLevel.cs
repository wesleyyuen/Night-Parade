using UnityEngine;

public class OnTriggerLoadLevel : MonoBehaviour {
    [SerializeField] private string levelToLoad = "";
    private void OnTriggerEnter2D (Collider2D collider) {
        if (collider.CompareTag ("Player")) {
            FindObjectOfType<GameMaster> ().requestSceneChange (levelToLoad, FindObjectOfType<PlayerHealth> ().currHealth, FindObjectOfType<PlayerInventory>().coinOnHand);
        }
    }
}