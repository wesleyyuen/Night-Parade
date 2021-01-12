using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [SerializeField] protected float movementSpeed;
    protected Collider2D player;
    protected Rigidbody2D rb;
    protected Enemy enemy;
    protected EnemyGFX enemyGFX;
    protected EnemyAggression enemyAggression;

    protected virtual void Start () {
        enemy = GetComponent<Enemy> ();
        enemyGFX = GetComponent<EnemyGFX> ();
        enemyAggression = GetComponent<EnemyAggression>();
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D> ();

    }

    protected virtual void FixedUpdate () {
        if (player == null || enemy.isTakingDmg || enemy.isDead) return;

        if (enemyAggression.GetIsAggro()) {
            // Only move after enemy is done turning
            if (!enemyGFX.GetIsTurning() && !enemy.collisionOnCooldown) {
                MoveTowardsPlayer();
            }
        }
    }

    protected virtual void MoveTowardsPlayer() {
        Vector2 target = new Vector2 (player.bounds.center.x, rb.position.y);
        
        Vector2 direction = (target - (Vector2) transform.position).normalized;
        rb.velocity = new Vector2(direction.x * movementSpeed , rb.velocity.y);
    }

}