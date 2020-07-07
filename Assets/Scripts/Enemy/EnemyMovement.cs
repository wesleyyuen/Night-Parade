using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [HideInInspector] public Vector2 patrolOrigin;
    [SerializeField] private float aggroDistance;
    public float movementSpeed;
    private Transform player;
    private Rigidbody2D rb;
    private Enemy enemy;

    void Start () {
        enemy = GetComponent<Enemy> ();
        player = enemy.player;
        rb = enemy.rb;
    }

    void Update () {
        if (player == null) return;
        
        if (Vector2.Distance (player.position, rb.position) < aggroDistance && !enemy.collisionOnCooldown) {
            Vector2 target = new Vector2 (player.position.x, rb.position.y);
            Vector2 newPosition = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.deltaTime);
            rb.position = newPosition;
        }
    }
}