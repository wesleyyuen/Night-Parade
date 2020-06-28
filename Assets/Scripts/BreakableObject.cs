using UnityEngine;

public class BreakableObject : MonoBehaviour {

    public int numOfHitsToDestroy;
    public enum breakableSide {
        left,
        right,
        both
    }
    public breakableSide side;
    int currentHealth;
    Rigidbody2D rb;

    void Start () {
        currentHealth = numOfHitsToDestroy;
        rb = GetComponent<Rigidbody2D> ();
    }

    public void TakeDamage (GameObject player) {
        float dir = player.transform.position.x - gameObject.transform.position.x;
        if (side == breakableSide.both || // attack from either side is fine
            (dir < 0 && side == breakableSide.left) || // attack from left
            (dir > 0 && side == breakableSide.right)) { // attack from right
            currentHealth--;
            if (currentHealth <= 0) {
                Break ();
            }
        } else {
            // Play blade clink sword effect
        }
    }

    void Break () {
        Destroy (gameObject);
    }
}