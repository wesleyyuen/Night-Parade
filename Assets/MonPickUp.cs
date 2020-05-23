using UnityEngine;

public class MonPickUp : MonoBehaviour {

    public int amount;
    void Start () {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);
    }

    private void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject.CompareTag ("Player")) {
            other.gameObject.GetComponent<PlayerInventory> ().pickUpCoin (amount);
            Destroy (gameObject);
        }
    }
}