using UnityEngine;

public class MonPickUp : PickUp {

    [SerializeField] private int amount;
    public override void Start () {
        base.Start();
    }

    private void OnCollisionEnter2D (Collision2D other) {
        if (other.gameObject.CompareTag ("Player")) {
            other.gameObject.GetComponent<PlayerInventory> ().PickUpCoin (amount);
            Destroy (gameObject);
        }
    }
}