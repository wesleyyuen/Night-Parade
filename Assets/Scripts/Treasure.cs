using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : BreakableObject
{

    [SerializeField] int numOfMonsPerHit;
    [SerializeField] GameObject mon;
    [SerializeField] float forceMultiplier;

    public override bool TakeDamage(float damage, Vector2 direction)
    {
        currentHealth--;

        // Spawn coins on every hit
        for (int i = 0; i < numOfMonsPerHit; i++) {
            GameObject coin = Instantiate (mon, gameObject.transform.position, Quaternion.identity);
            coin.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (direction.x > 0f ? -1 : 1 * forceMultiplier, forceMultiplier * Random.value));
        }
        if (currentHealth <= 0) {
            Break ();
        }

        return true;
    }
}