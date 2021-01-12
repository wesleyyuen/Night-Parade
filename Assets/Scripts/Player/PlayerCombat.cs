using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private float baseDamage;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Transform upThrustPoint;
    [SerializeField] private Transform downThrustPoint;
    [SerializeField] private Vector2 attackRange;
    [SerializeField] private Vector2 upThrustRange;
    [SerializeField] private Vector2 downThrustRange;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float comboTimeframeAfterAttack;
    [SerializeField] private float horizontalKnockBackForce = 5f;
    [SerializeField] private float verticalKnockBackForce = 20f;
    [HideInInspector] public bool canAttack { get; set; }
    private float nextAttackTime = 0f;
    private int comboCounter;
    private bool nextCombo;
    private bool isInComboTimeframe;
    private const int maxComboCount = 3;
    private List<int> enemiesAttackedIDs;

    // Hashing strings for optimization, but seems to make player falling animaton not play during New Game
    // private int attackHash = Animator.StringToHash ("Attack"); TODO: Not working

    private void Awake() {
        player = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        canAttack = true;
        nextCombo = false;
        isInComboTimeframe = false;
    }

    private void Update () {
        if (canAttack && Input.GetButtonDown ("Attack")) {
            // Handle Combo
            if (isInComboTimeframe && comboCounter != 0)
                nextCombo = true;

            if (Time.time >= nextAttackTime) {
                // Start First Attack
                if (comboCounter == 0) {
                    animator.SetInteger ("Attack", 1);
                    comboCounter = 1;
                }

                // Make a list of hit enemies/breakables so it won't double-count
                enemiesAttackedIDs = new List<int> ();

                if (Input.GetAxisRaw ("Vertical") > 0) {
                    UpThrust ();
                    return;
                }

                if (Input.GetAxisRaw ("Vertical") < 0 && !FindObjectOfType<PlayerPlatformCollision> ().onGround) {
                    DownThrust ();
                    return;
                }
            }
            
        }
    }

    // Called from Animation frames
    void Attack () {
        // Get Colliders of enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (attackPoint.position, attackRange, 360, enemyLayers);

        if (hitEnemies.Length == 0) return;

        foreach (Collider2D enemy in hitEnemies) {
            // Damage enemy/breakables only ONCE by adding them into list
            if (!enemiesAttackedIDs.Contains (enemy.gameObject.GetInstanceID ())) {
                enemiesAttackedIDs.Add (enemy.gameObject.GetInstanceID ());
                if (enemy.GetComponent<Enemy> () != null)
                    enemy.GetComponent<Enemy> ().TakeDamage (baseDamage);

                if (enemy.GetComponent<BreakableObject> () != null)
                    enemy.GetComponent<BreakableObject> ().TakeDamage (gameObject);
            }
        }
        rb.AddForce (new Vector2 (horizontalKnockBackForce * -player.localScale.x, 0.0f), ForceMode2D.Impulse);
    }

    void AttackAndListenForNextCombo() {
        isInComboTimeframe = true;
        Attack();
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
                    enemy.GetComponent<Enemy> ().TakeDamage (baseDamage);

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
                    enemy.GetComponent<Enemy> ().TakeDamage (baseDamage);

                if (enemy.GetComponent<BreakableObject> () != null)
                    enemy.GetComponent<BreakableObject> ().TakeDamage (gameObject);
            }
        }
        rb.AddForce (new Vector2 (0.0f, verticalKnockBackForce), ForceMode2D.Impulse);
    }

    // Called from animation frame
    void EndAttack () {
        StartCoroutine(Common.ChangeVariableAfterDelay<bool>(e => isInComboTimeframe = e, comboTimeframeAfterAttack, true, false));

        if (nextCombo && comboCounter < maxComboCount) {
            nextCombo = false;
            comboCounter++;
            animator.SetInteger ("Attack", comboCounter);
            nextAttackTime = Time.time + attackCooldown;
        } else {
            animator.SetInteger ("Attack", 0);
            comboCounter = 0;
            nextAttackTime = Time.time + attackCooldown * 5;
        }

        enemiesAttackedIDs.Clear ();
    }

    void EndUpThrust () {
        animator.SetBool ("UpThrust", false);
        nextAttackTime = Time.time + attackCooldown;
        enemiesAttackedIDs.Clear ();
    }

    void EndDownThrust () {
        animator.SetBool ("DownThrust", false);
        nextAttackTime = Time.time + attackCooldown;
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