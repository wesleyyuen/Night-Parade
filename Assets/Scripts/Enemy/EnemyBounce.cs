using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBounce : EnemyMovement {
    public float bounceForce;
    private EnemyGrounded enemyGroundCheck;

    protected override void Start () {
        base.Start();
        enemyGroundCheck = GetComponentInChildren<EnemyGrounded> ();
    }

    protected override void Update() {
        if (player == null) return;

        if (Vector2.Distance (player.position, rb.position) < aggroDistance && !enemy.collisionOnCooldown) {
            if (!isAggro) {
                isAggro = true;
                StartCoroutine (FlashExclaimationMark ());
            }
        } else {
            isAggro = false;
        }
    }

    public void Bounce () {
        if (Vector2.Distance (player.position, rb.position) < aggroDistance && !enemy.collisionOnCooldown && enemyGroundCheck.isGrounded) {
            rb.AddForce (new Vector2 (transform.localScale.x * movementSpeed, bounceForce), ForceMode2D.Impulse);
        }
    }
}