using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    public Transform player;
    public Animator animator;
    public Rigidbody2D rb;
    public Transform attackPoint;
    public Transform upThrustPoint;
    public Transform downThrustPoint;
    public Vector2 attackRange;
    public Vector2 upThrustRange;
    public Vector2 downThrustRange;
    public LayerMask enemyLayers;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public float horizontalKnockBackForce = 5f;
    public float verticalKnockBackForce = 20f;

    List<int> enemiesAttackedIDs;

    void Update () {
        if (Time.time >= nextAttackTime && Input.GetButtonDown ("Attack")) {
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
            animator.SetBool ("Attack", true);  // Start attack animation, Attack() will be called from the animation frames
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    void Attack () {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (attackPoint.position, attackRange, 360, enemyLayers);

        if (hitEnemies.Length == 0) return;

        foreach (Collider2D enemy in hitEnemies) {
            if (!enemiesAttackedIDs.Contains(enemy.gameObject.GetInstanceID ())) {
                if (enemy.GetComponent<Enemy> () != null) {
                enemiesAttackedIDs.Add (enemy.gameObject.GetInstanceID ());
                enemy.GetComponent<Enemy> ().TakeDamage ();
                continue;
            }
            enemy.GetComponent<BreakableObject> ().TakeDamage ();
            }
        }
        rb.AddForce (new Vector2 (horizontalKnockBackForce * -player.localScale.x, 0.0f), ForceMode2D.Impulse);
    }

    void UpThrust () {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (upThrustPoint.position, upThrustRange, 360, enemyLayers);

        animator.SetBool ("UpThrust", true);

        if (hitEnemies.Length == 0) return;

        foreach (Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy> ().TakeDamage ();
        }
    }

    void DownThrust () {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (downThrustPoint.position, downThrustRange, 360, enemyLayers);

        animator.SetBool ("DownThrust", true);

        if (hitEnemies.Length == 0) return;

        foreach (Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy> ().TakeDamage ();
        }
        rb.AddForce (new Vector2 (0.0f, verticalKnockBackForce), ForceMode2D.Impulse);
    }

    void EndAttack () {
        animator.SetBool ("Attack", false);
        enemiesAttackedIDs.Clear();
    }

    void EndUpThrust () {
        animator.SetBool ("UpThrust", false);
    }

    void EndDownThrust () {
        animator.SetBool ("DownThrust", false);
    }

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