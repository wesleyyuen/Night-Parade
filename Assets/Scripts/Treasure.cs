using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : BreakableObject {

    [SerializeField] private int numOfMonsPerHit;
    [SerializeField] private GameObject mon;
    [SerializeField] private float forceMultiplier;

    public override void TakeDamage (GameObject player) {
        float dir = player.transform.position.x - gameObject.transform.position.x;
        currentHealth--;

        // Spawn coins on every hit
        for (int i = 0; i < numOfMonsPerHit; i++) {
            GameObject coin = Instantiate (mon, gameObject.transform.position, Quaternion.identity);
            coin.GetComponent<Rigidbody2D> ().AddForce (new Vector2 ((dir < 0) ? -1 : 1 * forceMultiplier, forceMultiplier * Random.value));
        }
        if (currentHealth <= 0) {
            Break ();
        }
    }
}