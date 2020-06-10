using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    public int currHealth { get; private set; }
    public int maxNumOfHeart { get; private set; }
    Transform player;
    Rigidbody2D rb;

    void Start () {
        currHealth = FindObjectOfType<GameMaster> ().savedPlayerData.SavedPlayerHealth;
        maxNumOfHeart = FindObjectOfType<GameMaster> ().savedPlayerData.SavedMaxPlayerHealth;
        player = gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage (float takeDamageKnockBackForce) {
        rb.AddForce (new Vector2 (takeDamageKnockBackForce * -player.localScale.x, 0.0f), ForceMode2D.Impulse);
        currHealth--;
        Debug.Log ("Current Health: " + currHealth);
        if (currHealth <= 0) {
            Die();
        }
    }

    public void PickUpHealth () {
        if (currHealth < maxNumOfHeart) {
            currHealth++;
        }
    }

    void Die () {
        Destroy (gameObject);
        Debug.Log ("You died");
    }
}