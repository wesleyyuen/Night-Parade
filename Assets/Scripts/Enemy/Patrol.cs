using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour { // Has to be on same level as enemy

    [SerializeField] private float patrolDistance;
    [HideInInspector] public Vector2 patrolOrigin;
    private float aggroDistance;
    private float movementSpeed;
    private Transform player;
    private Rigidbody2D rb;
    private bool isPatroling;

    void Start () { // use start becuase it depends on Enemy
        Enemy enemy = GetComponent<Enemy> ();
        EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        aggroDistance = enemy.aggroDistance;
        movementSpeed = enemyMovement.movementSpeed;
        player = enemy.player;
        rb = enemy.rb;
        isPatroling = false;
    }

    void Update () {
        if (player == null) return;
        
        if (Vector2.Distance (player.position, rb.position) < aggroDistance) {
            isPatroling = false;
        } else {
            if (!isPatroling) {
                patrolOrigin = rb.position;
                isPatroling = true;
            }
            
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
    }
}