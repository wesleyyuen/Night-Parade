using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public int maxHealth;
    public float aggroDistance;
    public float movementSpeed;
    public float knockBackForce;
    int currentHealth;
    Transform player;
    Rigidbody2D rb;
    void Start () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = GetComponent<Rigidbody2D> ();
    }

    void Update () {
        if (player != null && Vector2.Distance (player.position, transform.position) < aggroDistance) {
            move ();
        }
    }

    void move () {
        Vector2 target = new Vector2 (player.position.x, rb.position.y);
        Vector2 newPosition = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.fixedDeltaTime);
        rb.position = newPosition;
    }

    public void TakeDamage () {
        // TODO Animation
        rb.AddForce(new Vector2(knockBackForce * -transform.localScale.x, 0f), ForceMode2D.Impulse);
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