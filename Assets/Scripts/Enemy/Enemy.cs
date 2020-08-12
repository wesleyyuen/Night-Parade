using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour { // handle ONLY collision and health
    [Header ("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem deathParticleEffect;
    [HideInInspector] public Transform player;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    [Header ("Behaviors")]

    [SerializeField] public int maxHealth;
    public float aggroDistance; // keep this here for miniboss (miniboss use different movement)
    [SerializeField] public float knockBackOnAttackForce;
    [SerializeField] public float collisionKnockBackForceOnPlayer;
    [SerializeField] public float collisionCooldown = 1.5f;

    [SerializeField] public float dieTime;
    [HideInInspector] public int currentHealth;

    [HideInInspector] public bool collisionOnCooldown;
    [SerializeField] private float flashDuration;

    private float startTime;
    private float timer;
    public bool isDead { private set; get; }

    public virtual void Start () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = GetComponent<Rigidbody2D> ();
        sr = GetComponent<SpriteRenderer> ();
        collisionOnCooldown = false;
        isDead = false;
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

    private void OnBecameVisible () {
        enabled = true;
    }

    private void OnBecameInvisible () {
        enabled = false;
    }

    public virtual void OnCollisionStay2D (Collision2D collision) {
        if (collisionOnCooldown) return;

        // Collide with player, then enemy freezes and player takes damage and knockback 
        if (collision.gameObject.layer == LayerMask.NameToLayer ("Player") && !collisionOnCooldown) {
            collisionOnCooldown = true;
            startTime = Time.time;
            timer = startTime;
            collision.gameObject.GetComponent<PlayerHealth> ().TakeDamage (rb.position, collisionKnockBackForceOnPlayer);
        }
    }

    public virtual void TakeDamage () {
        // TODO Animation
        Vector2 knockBackDirection = Vector3.Normalize (rb.position - (Vector2) player.position);
        rb.AddForce (knockBackOnAttackForce * knockBackDirection, ForceMode2D.Impulse);
        //GetComponent<SpriteRenderer> ().color = new Color (0.69f, 0.16f, 0.16f, 1.0f);
        StartCoroutine (DamagedEffect ());
        currentHealth--;
        if (currentHealth <= 0) {
            isDead = true;
            deathParticleEffect.Play ();
            StartCoroutine (Die ());
        }
    }

    public virtual IEnumerator DamagedEffect () {
        // TODO: fix changing colors for slime
        GetComponent<SpriteRenderer> ().color = new Color (0.69f, 0.16f, 0.16f, 1.0f);

        yield return new WaitForSeconds (flashDuration);
        GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
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