using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public int currHealth { get; private set; }
    public int maxHealth { get; private set; }
    
    private Rigidbody2D rb;
    private GameMaster gameMaster;
    [HideInInspector] public bool isInvulnerable;
    [SerializeField] private float damageKnockBackMultiplier;
    [SerializeField] private float damageCameraShakeMultiplier;
    [SerializeField] private float damageCameraShakeTimer;

    void Start () {
        gameMaster = FindObjectOfType<GameMaster> ();
        currHealth = gameMaster.savedPlayerData.SavedPlayerHealth;
        maxHealth = gameMaster.savedPlayerData.SavedMaxPlayerHealth;
        rb = GetComponent<Rigidbody2D> ();

        // Change Health UI
        FindObjectOfType<HealthUI>().UpdateHearts();
    }

    public void TakeDamage (int damage, Vector2 enemyPos) {
        if (isInvulnerable) return;

        currHealth -= damage;
        if (currHealth <= 0) {
            Die ();
        }

        // Change Health UI
        FindObjectOfType<HealthUI>().UpdateHearts();

        // Apply knockback force to player in opposite direction based on damage amount
        Vector2 knockBackDirection = Vector3.Normalize (rb.position - enemyPos);
        rb.AddForce (damage * damageKnockBackMultiplier * knockBackDirection, ForceMode2D.Impulse);

        // Shake Camera based on damage amount
        CameraShake.Instance.ShakeCamera(damage * damageCameraShakeMultiplier, damageCameraShakeTimer);

        // Invulnerability Frames
        FindObjectOfType<InvulnerabilityFrames> ().Flash ();
    }

    public void PickUpHealth () {
        if (currHealth < maxHealth) {
            currHealth += 4;
        }

        // Change Health UI
        FindObjectOfType<HealthUI>().UpdateHearts();
    }

    public void FullHeal () {
        currHealth = maxHealth;

        // Change Health UI
        FindObjectOfType<HealthUI>().UpdateHearts();
    }

    void Die () {
        Destroy (gameObject);
        Debug.Log ("You died");
        SceneManager.LoadScene ("Main_Menu");

        // TODO: probably a game over screen
    }
}