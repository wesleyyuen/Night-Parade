using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public int currHealth { get; private set; }
    public int maxNumOfHeart { get; private set; }

    [SerializeField] private Transform player;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameMaster gameMaster;
    [HideInInspector] public bool isInvulnerable;

    void Start () {
        gameMaster = FindObjectOfType<GameMaster> ();
        currHealth = gameMaster.savedPlayerData.SavedPlayerHealth;
        maxNumOfHeart = gameMaster.savedPlayerData.SavedMaxPlayerHealth;
        player = gameObject.transform;
        rb = GetComponent<Rigidbody2D> ();
    }

    public void TakeDamage (Vector2 enemyPos, float takeDamageKnockBackForce) {
        if (isInvulnerable) return;

        currHealth--;
        Debug.Log ("Current Health: " + currHealth);
        if (currHealth <= 0) {
            Die ();
        }

        // Apply knockback force to player in opposite direction
        Vector2 knockBackDirection = Vector3.Normalize (rb.position - enemyPos);
        rb.AddForce (takeDamageKnockBackForce * knockBackDirection, ForceMode2D.Impulse);

        // Invulnerability Frames
        FindObjectOfType<InvulnerabilityFrames> ().Flash ();
    }

    public void PickUpHealth () {
        if (currHealth < maxNumOfHeart) {
            currHealth++;
        }
    }

    public void FullHeal () {
        currHealth = maxNumOfHeart;
    }

    void Die () {
        Destroy (gameObject);
        Debug.Log ("You died");
        SceneManager.LoadScene ("Main_Menu");

        // TODO: probably a game over screen
    }
}