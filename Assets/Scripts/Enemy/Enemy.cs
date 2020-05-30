using UnityEngine;

public class Enemy : MonoBehaviour {

    public int maxHealth;
    public float aggroDistance;
    public float movementSpeed;
    public float knockBackForce;
    int currentHealth;
    Transform player;
    Rigidbody2D rb;
    bool collisionOnCooldown;
    float startTime;
    float timer;
    public float collisionCooldown = 1.5f;
    public float patrolDistance;
    bool isPatroling;
    Vector2 patrolOrigin;

    void Awake () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = GetComponent<Rigidbody2D> ();
        collisionOnCooldown = false;
    }

    void Update () {
        // Freeze for collisionCooldown after colliding with player
        if (collisionOnCooldown) {
            timer += Time.deltaTime;
            if (timer > startTime + collisionCooldown) {
                collisionOnCooldown = false;
            }
        }
        if (player == null || collisionOnCooldown) return;
        if (Vector2.Distance (player.position, rb.position) < aggroDistance) {
            isPatroling = false;
            MoveTowardsPlayer ();
        } else {
            if (!isPatroling) {
                patrolOrigin = rb.position;
                isPatroling = true;
            }
            Patrol ();
        }
    }

    void MoveTowardsPlayer () {
        Vector2 target = new Vector2 (player.position.x, rb.position.y);
        Vector2 newPosition = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.deltaTime);
        rb.position = newPosition;
    }

    void Patrol () {
        float leftBound = patrolOrigin.x - patrolDistance;
        float rightBound = patrolOrigin.x + patrolDistance;
        Vector2 target = rb.position;
        if (rb.position.x <= leftBound) {
            transform.localScale = new Vector3 (1f, 1f, 1f);
        } else if (rb.position.x >= rightBound) {
            transform.localScale = new Vector3 (-1f, 1f, 1f);
        }
        if (transform.localScale.x == 1f) { // facing right
            target = new Vector2 (rightBound, rb.position.y);
        } else if (transform.localScale.x == -1f) { // facing left
            target = new Vector2 (leftBound, rb.position.y);
        }
        Vector2 newPositionR = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.deltaTime);
        rb.position = newPositionR;
    }

    void OnCollisionEnter2D (Collision2D collision) {
        if (collisionOnCooldown) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer ("Player") && !collisionOnCooldown) {
            collisionOnCooldown = true;
            startTime = Time.time;
            timer = startTime;
            collision.gameObject.GetComponent<PlayerHealth> ().TakeDamage (35f);
        }
    }

    public void TakeDamage () {
        // TODO Animation
        rb.AddForce (new Vector2 (knockBackForce * -transform.localScale.x, 0f), ForceMode2D.Impulse);
        currentHealth--;
        if (currentHealth <= 0) {
            Die ();
        }
    }

    void Die () {
        // TODO Animation
        Debug.Log (gameObject.name + " Died");
        GetComponent<EnemyDrop> ().SpawnDrops ();
        Destroy (gameObject);
    }
}