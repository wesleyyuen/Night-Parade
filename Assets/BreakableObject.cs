using UnityEngine;

public class BreakableObject : MonoBehaviour {

    public int numOfHitsToDestroy;
    public float knockBackForce;
    int currentHealth;
    Rigidbody2D rb;

    void Start () {
        currentHealth = numOfHitsToDestroy;
        rb = GetComponent<Rigidbody2D> ();
    }

    public void TakeDamage () {
        // TODO Animation
        rb.AddForce (new Vector2 (knockBackForce * -transform.localScale.x, 0f), ForceMode2D.Impulse);
        currentHealth--;
        if (currentHealth <= 0) {
            Break ();
        }
    }

    void Break () {
        Destroy (gameObject);
    }
}