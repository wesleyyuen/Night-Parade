using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbPickUp : PickUp
{
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player")) {
            other.gameObject.GetComponent<PlayerInventory> ().PickUpOrb ();
            Destroy (gameObject);
        }
    }
}