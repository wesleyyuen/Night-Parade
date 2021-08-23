using UnityEngine;

public class HealthPickUp : PickUp
{
    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag ("Player")) {
            other.gameObject.GetComponent<PlayerHealth> ().PickUpHealth();
            Destroy (gameObject);
        }
    }
}