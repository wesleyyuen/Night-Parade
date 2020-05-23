using UnityEngine;

public class HealthPickUp : MonoBehaviour {

    void Start () {
        Physics2D.IgnoreLayerCollision(gameObject.layer, LayerMask.NameToLayer("Enemies"), true);
    }

    private void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject.CompareTag ("Player")) {
            other.gameObject.GetComponent<PlayerHealth> ().PickUpHealth();
            Destroy (gameObject);
        }
    }
}