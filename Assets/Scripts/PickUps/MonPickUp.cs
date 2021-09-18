using UnityEngine;

public class MonPickUp : PickUp
{
    [SerializeField] int amount;

    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag ("Player")) {
            other.gameObject.GetComponent<PlayerInventory>().PickUpCoin (amount);

            // Update Mon UI
            MonUI.Instance.ShowMonChange();

            Destroy(gameObject);
        }
    }
}