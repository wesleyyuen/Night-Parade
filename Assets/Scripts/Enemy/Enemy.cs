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

    void Awake () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = GetComponent<Rigidbody2D> ();
        collisionOnCooldown = false;
    }

    void Update () {
        if (player != null && Vector2.Distance (player.position, transform.position) < aggroDistance && !collisionOnCooldown) {
            Move ();
        }
        if (collisionOnCooldown) {
            timer += Time.deltaTime;
            if (timer > startTime + collisionCooldown) {
                collisionOnCooldown = false;
            }
        }
    }

    void Move () {
        Vector2 target = new Vector2 (player.position.x, rb.position.y);
        Vector2 newPosition = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.fixedDeltaTime);
        rb.position = newPosition;
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
        Destroy (gameObject);
    }
}