using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour { // Has to be on same level as enemy

    [SerializeField] private float patrolSpeed;
    //[SerializeField] private float groundDetectionOffset
    private Transform player;
    private Collider2D col;
    private Rigidbody2D rb;
    private EnemyMovement enemyMovement;
    private EnemyAggression enemyAggression;
    private EnemyGFX enemyGFX;

    void Start () { // use start becuase it depends on Enemy
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAggression = GetComponent<EnemyAggression>();
        enemyGFX = GetComponent<EnemyGFX>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }


    //  TODO - Fixedupdate: change to handle physics movement better in FixedUpdate
    void Update () {
        if (player == null) return;
        
        // Only patrol if enemy is not aggroing
        if (!enemyAggression.GetIsAggro()) {
            Vector2 target = transform.position;
            Vector2 groundDetectionPoint = new Vector2 (transform.localScale.x == 1f ? col.bounds.max.x : col.bounds.min.x, col.bounds.center.y);

            RaycastHit2D hit = Physics2D.Raycast(groundDetectionPoint, -transform.up, col.bounds.size.y, LayerMask.GetMask("Ground"));
            Debug.DrawRay(groundDetectionPoint, - transform.up * col.bounds.size.y, Color.green);

            if (!hit || !hit.collider.CompareTag("Ground")) {
                StartCoroutine(enemyGFX.PatrolTurnAround(true));
                groundDetectionPoint = new Vector2 (transform.localScale.x == 1f ? col.bounds.min.x : col.bounds.max.x, col.bounds.center.y);
            }
            target = new Vector2 (groundDetectionPoint.x, rb.position.y);

            // Move towards target location
            rb.position = Vector2.MoveTowards (rb.position, target, patrolSpeed * Time.deltaTime);
        }

    }
}