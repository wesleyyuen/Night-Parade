using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounce : EnemyMovement {
    public float bounceForce;
    private EnemyGrounded enemyGroundCheck;

    protected override void Start () {
        base.Start();
        enemyGroundCheck = GetComponentInChildren<EnemyGrounded> ();
        enemyAggression = GetComponent<EnemyAggression>();
    }

    public void Bounce () {
        if (enemyAggression.GetIsAggro() && !enemy.collisionOnCooldown && enemyGroundCheck.isGrounded) {
            rb.AddForce (new Vector2 (transform.localScale.x * movementSpeed, bounceForce), ForceMode2D.Impulse);
        } else {
            rb.AddForce (new Vector2 (0.0f, bounceForce), ForceMode2D.Impulse);
        }
    }
}