﻿using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform upThrustPoint;
    [SerializeField] private Transform downThrustPoint;
    [SerializeField] private Vector2 attackRange;
    [SerializeField] private Vector2 upThrustRange;
    [SerializeField] private Vector2 downThrustRange;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackRate = 2f;
    [SerializeField] private float nextAttackTime = 0f;
    [SerializeField] private float horizontalKnockBackForce = 5f;
    [SerializeField] private float verticalKnockBackForce = 20f;

    private List<int> enemiesAttackedIDs;

    // Hashing strings for optimization, but seems to make player falling animaton not play during New Game
    // private int attackHash = Animator.StringToHash ("Attack"); TODO: Not working


    // TODO: Fixedupdate: change to handle forces in FixedUpdate
    void Update () {
        if (Time.time >= nextAttackTime && Input.GetButtonDown ("Attack")) {
            // Make a list of hit enemies/breakables so it won't double-count
            enemiesAttackedIDs = new List<int> ();

            if (Input.GetAxisRaw ("Vertical") > 0) {
                UpThrust ();
                nextAttackTime = Time.time + 1f / attackRate;
                return;
            }

            if (Input.GetAxisRaw ("Vertical") < 0 && !FindObjectOfType<Grounded> ().isGrounded) {
                DownThrust ();
                nextAttackTime = Time.time + 1f / attackRate;
                return;
            }

            // Start attack animation, Attack() will be called from the animation frames
            animator.SetBool ("Attack", true);

            // Cooldown for attacks
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack () {
        // Get Colliders of enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (attackPoint.position, attackRange, 360, enemyLayers);

        if (hitEnemies.Length == 0) return;

        foreach (Collider2D enemy in hitEnemies) {
            // Damage enemy/breakables only ONCE by adding them into list
            if (!enemiesAttackedIDs.Contains (enemy.gameObject.GetInstanceID ())) {
                enemiesAttackedIDs.Add (enemy.gameObject.GetInstanceID ());
                if (enemy.GetComponent<Enemy> () != null)
                    enemy.GetComponent<Enemy> ().TakeDamage ();

                if (enemy.GetComponent<BreakableObject> () != null)
                    enemy.GetComponent<BreakableObject> ().TakeDamage (gameObject);
            }
        }
        rb.AddForce (new Vector2 (horizontalKnockBackForce * -player.localScale.x, 0.0f), ForceMode2D.Impulse);
    }

    void UpThrust () {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (upThrustPoint.position, upThrustRange, 360, enemyLayers);

        animator.SetBool ("UpThrust", true);

        if (hitEnemies.Length == 0) return;

        foreach (Collider2D enemy in hitEnemies) {
            // Damage enemy/breakables only ONCE by adding them into list
            if (!enemiesAttackedIDs.Contains (enemy.gameObject.GetInstanceID ())) {
                enemiesAttackedIDs.Add (enemy.gameObject.GetInstanceID ());
                if (enemy.GetComponent<Enemy> () != null)
                    enemy.GetComponent<Enemy> ().TakeDamage ();

                if (enemy.GetComponent<BreakableObject> () != null)
                    enemy.GetComponent<BreakableObject> ().TakeDamage (gameObject);
            }
        }
    }

    void DownThrust () {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (downThrustPoint.position, downThrustRange, 360, enemyLayers);

        animator.SetBool ("DownThrust", true);

        if (hitEnemies.Length == 0) return;

        foreach (Collider2D enemy in hitEnemies) {
            // Damage enemy/breakables only ONCE by adding them into list
            if (!enemiesAttackedIDs.Contains (enemy.gameObject.GetInstanceID ())) {
                enemiesAttackedIDs.Add (enemy.gameObject.GetInstanceID ());
                if (enemy.GetComponent<Enemy> () != null)
                    enemy.GetComponent<Enemy> ().TakeDamage ();

                if (enemy.GetComponent<BreakableObject> () != null)
                    enemy.GetComponent<BreakableObject> ().TakeDamage (gameObject);
            }
        }
        rb.AddForce (new Vector2 (0.0f, verticalKnockBackForce), ForceMode2D.Impulse);
    }

    // Called from animation frame
    void EndAttack () {
        animator.SetBool ("Attack", false);
        enemiesAttackedIDs.Clear ();
    }

    void EndUpThrust () {
        animator.SetBool ("UpThrust", false);
        enemiesAttackedIDs.Clear ();
    }

    void EndDownThrust () {
        animator.SetBool ("DownThrust", false);
        enemiesAttackedIDs.Clear ();
    }

    // visualize attacks ranges
    void OnDrawGizmosSelected () {
        if (attackPoint != null) {
            Gizmos.DrawWireCube (attackPoint.position, attackRange);
        }
        if (upThrustPoint != null) {
            Gizmos.DrawWireCube (upThrustPoint.position, upThrustRange);
        }
        if (downThrustPoint != null) {
            Gizmos.DrawWireCube (downThrustPoint.position, downThrustRange);
        }
    }
}