using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour { // handle ONLY collision and health
    [Header ("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem deathParticleEffect;
    protected Transform player;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [Header ("Behaviors")]

    [SerializeField] public float maxHealth;
    [SerializeField] public int damageAmount;
    [SerializeField] public float knockBackOnAttackForce;
    [SerializeField] public float collisionCooldown = 1.5f;

    [SerializeField] public float dieTime;
    [HideInInspector] public float currentHealth;

    [HideInInspector] public bool collisionOnCooldown;
    [SerializeField] private float flashDuration;

    private float startTime;
    private float timer;
    public bool isDead { private set; get; }
    public bool isTakingDmg { private set; get; }

    public virtual void Start () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = GetComponent<Rigidbody2D> ();
        sr = GetComponent<SpriteRenderer> ();
        collisionOnCooldown = false;
        isDead = false;
        isTakingDmg = false;
    }

    public virtual void Update () {
        if (isDead) return;

        // Freeze for collisionCooldown after colliding with player to avoid constantly colliding with player
        if (collisionOnCooldown) {
            timer += Time.deltaTime;
            if (timer > startTime + collisionCooldown) {
                collisionOnCooldown = false;
            }
        }
    }

    public virtual void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer ("Player"))
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public virtual void OnCollisionStay2D (Collision2D collision) {
        if (collisionOnCooldown) return;

        // Collide with player, then enemy freezes and player takes damage and knockback 
        if (collision.gameObject.layer == LayerMask.NameToLayer ("Player") && !collisionOnCooldown) {
            collisionOnCooldown = true;
            startTime = Time.time;
            timer = startTime;
            collision.gameObject.GetComponent<PlayerHealth> ().TakeDamage (damageAmount, rb.position);
        }
    }

    public virtual void OnCollisionExit2D (Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer ("Player"))
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public virtual void TakeDamage (float damage) {
        isTakingDmg = true;

        // TODO Animation
        Vector2 knockBackDirection = Vector3.Normalize (rb.position - (Vector2) player.position);
        rb.AddForce (knockBackOnAttackForce * knockBackDirection, ForceMode2D.Impulse);

        // Show Damaged Effect
        StartCoroutine (DamagedEffect ());

        currentHealth -= damage;

        // Death
        if (currentHealth <= 0) {
            isDead = true;
            StartCoroutine (Die ());
            deathParticleEffect.Play ();
        }
    }

    public virtual IEnumerator DamagedEffect () {
        // TODO: fix changing colors for slime
        GetComponent<SpriteRenderer> ().color = new Color (0.69f, 0.16f, 0.16f, 1.0f);

        yield return new WaitForSeconds (flashDuration);
        GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
        isTakingDmg = false;
    }

    public virtual IEnumerator Die () {
        // Ignore Player Collision to avoid player taking dmg when running into dying enemy
        Physics2D.IgnoreCollision (player.GetComponent<Collider2D> (), GetComponent<Collider2D> ());

        // Dying Animation
        for (float t = 0f; t < 1f; t += Time.deltaTime / dieTime) {
            sr.color = new Color (Mathf.Lerp (1, 0, t), Mathf.Lerp (1, 0, t), Mathf.Lerp (1, 0, t), 1.0f);
            yield return null;
        }

        //yield return new WaitForSeconds (dieTime);
        GetComponent<EnemyDrop> ().SpawnDrops ();
        Destroy (gameObject);
    }

}