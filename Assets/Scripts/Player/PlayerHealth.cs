using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public int currHealth { get; private set; }
    public int maxNumOfHeart { get; private set; }
    public float percentOfCoinsLostAfterDeath;
    Transform player;
    Rigidbody2D rb;
    GameMaster gameMaster;

    void Start () {
        gameMaster = FindObjectOfType<GameMaster> ();
        currHealth = gameMaster.savedPlayerData.SavedPlayerHealth;
        maxNumOfHeart = gameMaster.savedPlayerData.SavedMaxPlayerHealth;
        player = gameObject.transform;
        rb = GetComponent<Rigidbody2D> ();
    }

    public void TakeDamage (Vector2 enemyPos, float takeDamageKnockBackForce) {
        // TODO: fix knockback from above
        // Apply knockback force to player in opposite direction
        Vector2 knockBackDirection =  Vector3.Normalize(rb.position - enemyPos);
        rb.AddForce (takeDamageKnockBackForce * knockBackDirection, ForceMode2D.Impulse);

        currHealth--;
        Debug.Log ("Current Health: " + currHealth);
        if (currHealth <= 0) {
            Die ();
        }
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

        // TODO: returning to main menu in lieu of a game over screen
        /*
        SaveManager.SaveOnDeath (player.gameObject, percentOfCoinsLostAfterDeath);
        gameMaster.RequestSceneChange("")
        */
    }
}