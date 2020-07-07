using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {  // handle ONLY collision and health
    [Header ("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem deathParticleEffect;
    [HideInInspector] public Transform player;
    public Rigidbody2D rb;

    [Header ("Behaviors")]

    [SerializeField] public int maxHealth;
    public float aggroDistance;  // keep this here for miniboss (miniboss use different movement)
    [SerializeField] public float knockBackOnAttackForce;
    [SerializeField] public float collisionKnockBackForceOnPlayer;
    [SerializeField] public float collisionCooldown = 1.5f;
    
    [SerializeField] public float dieTime;
    [HideInInspector] public int currentHealth;

    [HideInInspector] public bool collisionOnCooldown;
    private float startTime;
    private float timer;
    private bool isDead;

    public virtual void Awake () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = GetComponent<Rigidbody2D> ();
        collisionOnCooldown = false;
        isDead = false;
    }

    public virtual void Update () {
        if (isDead) return;

        // Freeze for collisionCooldown after colliding with player
        if (collisionOnCooldown) {
            timer += Time.deltaTime;
            if (timer > startTime + collisionCooldown) {
                collisionOnCooldown = false;
            }
        }
    }

    public virtual void OnCollisionStay2D (Collision2D collision) {
        if (collisionOnCooldown) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer ("Player") && !collisionOnCooldown) {
            collisionOnCooldown = true;
            startTime = Time.time;
            timer = startTime;
            collision.gameObject.GetComponent<PlayerHealth> ().TakeDamage (rb.position, collisionKnockBackForceOnPlayer);
        }
    }

    public virtual void TakeDamage () {
        // TODO Animation
        Vector2 knockBackDirection =  Vector3.Normalize(rb.position - (Vector2) player.position);
        rb.AddForce (knockBackOnAttackForce * knockBackDirection, ForceMode2D.Impulse);

        currentHealth--;
        if (currentHealth <= 0) {
            isDead = true;
            deathParticleEffect.Play ();
            animator.SetTrigger ("Dead");
            StartCoroutine (Die ());
        }
    }

    public virtual IEnumerator Die () {
        // TODO Animation

        yield return new WaitForSeconds (dieTime);
        GetComponent<EnemyDrop>().SpawnDrops ();
        Destroy (gameObject);
    }

}