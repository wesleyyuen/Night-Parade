using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkPickUp : PickUp {

    [SerializeField] private string area;

    public override void Start () {
        bool pickedUpBefore;
        FindObjectOfType<PlayerProgress> ().areaProgress.TryGetValue (area + "_Ink", out pickedUpBefore);
        if (pickedUpBefore) Destroy (transform.parent.gameObject);
        base.Start ();
    }
    private void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag ("Player")) {
            other.GetComponent<PlayerInventory> ().PickUpInk (GameMaster.areaNameToIndex[area]);
            FindObjectOfType<PlayerProgress> ().areaProgress.Add (area + "_Ink", true);
            Destroy (gameObject);
        }
    }
}