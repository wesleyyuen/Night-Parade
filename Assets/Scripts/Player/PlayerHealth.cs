using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public int currHealth { get; private set; }
    public int maxHealth { get; private set; }
    
    private Rigidbody2D rb;
    [HideInInspector] public bool isInvulnerable;
    [SerializeField] private float damageKnockBackMultiplier;
    [SerializeField] private float cameraShakeMultiplier;
    [SerializeField] private float damageCameraShakeTimer;

    void Start () {
        currHealth = GameMaster.Instance.savedPlayerData.SavedPlayerHealth;
        maxHealth = GameMaster.Instance.savedPlayerData.SavedMaxPlayerHealth;
        rb = GetComponent<Rigidbody2D> ();

        // Change Health UI
        FindObjectOfType<HealthUI>().UpdateHearts();
    }

    public void TakeDamage (int damage, Vector2 enemyPos) {
        if (isInvulnerable) return;

        StartCoroutine( Common.ChangeVariableAfterDelay<float>(e => Time.timeScale = e, 0.15f, 0.5f, Time.timeScale));

        currHealth -= damage;
        if (currHealth <= 0) {
            Die ();
        }

        // Change Health UI
        FindObjectOfType<HealthUI>().UpdateHearts();



        // Apply knockback force to player in opposite direction based on damage amount
        Vector2 knockBackDirection = new Vector2 (rb.position.x > enemyPos.x ? 1f : -1f, 0.6f).normalized;
        Debug.Log(knockBackDirection);
        rb.velocity = Vector2.zero;
        rb.AddForce (damage * damageKnockBackMultiplier * knockBackDirection, ForceMode2D.Impulse);

        // Shake Camera based on damage amount
        CameraShake.Instance.ShakeCamera(damage * cameraShakeMultiplier, damageCameraShakeTimer);

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