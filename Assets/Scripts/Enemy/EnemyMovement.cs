using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [HideInInspector] public Vector2 patrolOrigin;
    public float aggroDistance;
    public float movementSpeed;
    Transform player;
    Rigidbody2D rb;
    Enemy enemy;

    void Start () {
        enemy = GetComponent<Enemy> ();
        player = enemy.player;
        rb = enemy.rb;
    }

    void Update () {
        if (Vector2.Distance (player.position, rb.position) < aggroDistance && !enemy.collisionOnCooldown) {
            Vector2 target = new Vector2 (player.position.x, rb.position.y);
            Vector2 newPosition = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.deltaTime);
            rb.position = newPosition;
        }
    }
}