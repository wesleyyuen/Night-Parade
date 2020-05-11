using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    public int currHealth { get; private set; }
    public Transform player;
    public Rigidbody2D rb;
    void Start () {
        currHealth = FindObjectOfType<GameMaster> ().savedPlayerHealth;
    }

    public void TakeDamage (float takeDamageKnockBackForce) {
        rb.AddForce (new Vector2 (takeDamageKnockBackForce * -player.localScale.x, 0.0f), ForceMode2D.Impulse);
        currHealth--;
        Debug.Log ("Current Health: " + currHealth);
    }

    void Update () {
        if (currHealth <= 0) {
            Die ();
        }
    }

    void Die () {
        Destroy (gameObject);
        Debug.Log ("You died");
    }
}