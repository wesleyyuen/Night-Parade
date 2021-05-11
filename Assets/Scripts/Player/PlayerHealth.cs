using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public int currHealth { get; private set; }
    public int maxHealth { get; private set; }
    
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerMovement movement;
    [HideInInspector] public bool isInvulnerable;
    [SerializeField] private float damageKnockBackMultiplier;
    [SerializeField] private float cameraShakeMultiplier;
    [SerializeField] private float damageCameraShakeTimer;

    void Start () {
        currHealth = GameMaster.Instance.savedPlayerData.SavedPlayerHealth;
        maxHealth = GameMaster.Instance.savedPlayerData.SavedMaxPlayerHealth;
        rb = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();

        // Change Health UI
        FindObjectOfType<HealthUI>().UpdateHearts();
    }

    public void TakeDamage (int damage, Vector2 enemyPos) {
        if (isInvulnerable) return;

        // Freeze frame effect
        StartCoroutine( Common.ChangeVariableAfterDelay<float>(e => Time.timeScale = e, 0.01f, 0.05f, Time.timeScale));

        currHealth -= damage;
        if (currHealth <= 0) {
            Die ();
            return;
        }

        // Change Health UI
        FindObjectOfType<HealthUI>().UpdateHearts();

        // Apply knockback force to player in opposite direction
        movement.EnablePlayerMovement(false, 0.35f);
        StartCoroutine(Common.ChangeVariableAfterDelay<float>(e => rb.drag = e, 0.1f, damageKnockBackMultiplier * 0.1f, 0));
        Vector2 knockBackDirection = new Vector2 (rb.position.x > enemyPos.x ? 1f : -1f, 1f).normalized;
        rb.AddForce (damageKnockBackMultiplier * knockBackDirection, ForceMode2D.Impulse);

        // Shake Camera
        CameraShake.Instance.ShakeCamera(cameraShakeMultiplier, damageCameraShakeTimer);

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

    private void Die () {
        Destroy (gameObject);
        Debug.Log ("You died");
        SceneManager.LoadScene ("Main_Menu");

        // TODO: probably a game over screen
    }
}