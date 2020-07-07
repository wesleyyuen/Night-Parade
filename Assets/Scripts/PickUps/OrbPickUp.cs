using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbPickUp : PickUp {

    public override void Start () {
        base.Start ();
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag ("Player")) {
            other.gameObject.GetComponent<PlayerInventory> ().PickUpOrb ();
            Destroy (gameObject);
        }
    }
}