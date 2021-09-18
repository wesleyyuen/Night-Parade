using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkPickUp : PickUp
{
    [SerializeField] string area;
    PlayerProgress _progress;

    protected override void Start ()
    {
        _progress = FindObjectOfType<PlayerProgress>();
        
        // Do not spawn if player already picked it up
        if (_progress.HasPlayerProgress(area + "_Ink"))
            Destroy (transform.parent.gameObject);

        base.Start ();
    }
    void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player")) {
            // Add to player Inventory
            other.GetComponent<PlayerInventory> ().PickUpInk (GameMaster.areaNameToIndex[area]);

            // Set flag so it won't spawn again
            _progress.AddPlayerProgress(area + "_Ink", 1);

            Destroy (gameObject);
        }
    }
}