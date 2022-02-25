using UnityEngine;

public class HealthPickUp : PickUp
{
    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.CompareTag ("Player")) {
            other.gameObject.GetComponent<PlayerHealthMO>().PickUpHealth();
            Destroy(gameObject);
        }
    }
}