using UnityEngine;

public class OnTriggerLoadLevel : MonoBehaviour {
    [SerializeField] private string levelToLoad = "";
    private void OnTriggerEnter2D (Collider2D collider) {
        if (collider.CompareTag ("Player")) {
            int health = FindObjectOfType<PlayerHealth> ().currHealth;
            int coins = FindObjectOfType<PlayerInventory>().coinOnHand;
            PlayerVariables playerVariables = new PlayerVariables(health, coins);
            FindObjectOfType<GameMaster> ().requestSceneChange (levelToLoad, playerVariables);
        }
    }
}