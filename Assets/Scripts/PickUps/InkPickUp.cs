using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkPickUp : PickUp
{
    [SerializeField] string area;

    public override void Start ()
    {
        // Do not spawn if player already picked it up
        bool pickedUpBefore;
        FindObjectOfType<PlayerProgress> ().areaProgress.TryGetValue (area + "_Ink", out pickedUpBefore);
        if (pickedUpBefore) Destroy (transform.parent.gameObject);

        base.Start ();
    }
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player")) {
            // Add to player Inventory
            other.GetComponent<PlayerInventory> ().PickUpInk (GameMaster.areaNameToIndex[area]);
            // Set flag so it won't spawn again
            FindObjectOfType<PlayerProgress> ().areaProgress.Add (area + "_Ink", true);
            Destroy (gameObject);
        }
    }
}