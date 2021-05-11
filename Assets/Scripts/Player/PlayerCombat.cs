using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement movement;
    private PlayerAnimations animations;
    [SerializeField] private float baseDamage;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackRange;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float horizontalKnockBackForce = 5f;
    [HideInInspector] public bool canAttack { get; set; } // for player controls
    [HideInInspector] public bool isAttacking {get; private set;}
    private float nextAttackTime = 0f;
    private bool isListeningForNextAttack;
    private bool hasNextAttack;
    private const int kMaxComboCount = 3;
    private List<int> enemiesAttackedIDs;

    // Hashing strings for optimization, but seems to make player falling animaton not play during New Game
    // private int attackHash = Animator.StringToHash ("Attack"); TODO: Not working

    private void Awake()
    {
        player = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        animations = GetComponent<PlayerAnimations>();
        enemiesAttackedIDs = new List<int> ();
        canAttack = true;
        isListeningForNextAttack = false;
        hasNextAttack = false;
    }

    public void EnablePlayerCombat(bool enabled, float time = 0f)
    {
        if (canAttack == enabled) return;

        if (time == 0)
            canAttack = enabled;
        else
            StartCoroutine(Common.ChangeVariableAfterDelay<bool>(e => canAttack = e, time, enabled, !enabled));
    }

    private void Update ()
    {
        if (Input.GetButtonDown ("Attack") ) {
            if (Time.time >= nextAttackTime && !isAttacking && canAttack) {
                BeginAttack();

                // Make a list of hit enemies/breakables so it won't double-count
                enemiesAttackedIDs = new List<int> ();

            } else if (isListeningForNextAttack) {
                hasNextAttack = true;
                isListeningForNextAttack = false;
            }
        }
    }

    void BeginAttack()
    {
        animator.SetInteger ("Attack", 1);
        movement.EnablePlayerMovement(false);
        movement.StepForward(1, 0.03f);
        isAttacking = true;
        canAttack = false;
    }

    public void StartListeningForNextAttack()
    {
        isListeningForNextAttack = true;
    }

    // Called from Animation frames
    void Attack ()
    {
        // Get Colliders of enemies hit
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll (attackPoint.position, attackRange, 360, enemyLayers);

        if (hitEnemies.Length == 0) return;

        bool attacked = false;
        foreach (Collider2D enemy in hitEnemies) {
            // Damage enemy/breakables only ONCE by adding them into list
            if (!enemiesAttackedIDs.Contains (enemy.gameObject.GetInstanceID ())) {
                enemiesAttackedIDs.Add (enemy.gameObject.GetInstanceID ());
                if (enemy.GetComponent<Enemy> () != null) {
                    attacked = true;
                    enemy.GetComponent<Enemy> ().TakeDamage (baseDamage);
                }

                if (enemy.GetComponent<BreakableObject> () != null) {
                    attacked = true;
                    enemy.GetComponent<BreakableObject> ().TakeDamage (gameObject);
                }
            }
        }

        if (attacked) {
            CameraShake.Instance.ShakeCamera(0.3f, 0.15f);
            rb.velocity = Vector2.zero;
            StartCoroutine(Common.ChangeVariableAfterDelay<float>(e => rb.drag = e, 0.1f, horizontalKnockBackForce * 0.1f, 0));
            rb.AddForce (new Vector2 (horizontalKnockBackForce * -player.localScale.x, 0.0f), ForceMode2D.Impulse);
        } 
    }

    // Called from animation frame
    public void EndAttack ()
    {
        if (hasNextAttack) {
            int nextAttack = (animator.GetInteger("Attack") % kMaxComboCount) + 1;
            animator.SetInteger ("Attack", nextAttack);
            movement.StepForward(1, 0.03f);
            hasNextAttack = false;
        }
        else {
            animator.SetInteger ("Attack", 0);
            movement.EnablePlayerMovement(true);
            isAttacking = false;
            canAttack = true;
        }

        isListeningForNextAttack = false;
        nextAttackTime = Time.time + attackCooldown;
        enemiesAttackedIDs.Clear ();
    }

    void EndUpThrust ()
    {
        animator.SetBool ("UpThrust", false);
        nextAttackTime = Time.time + attackCooldown;
        enemiesAttackedIDs.Clear ();
    }

    void EndDownThrust ()
    {
        animator.SetBool ("DownThrust", false);
        nextAttackTime = Time.time + attackCooldown;
        enemiesAttackedIDs.Clear ();
    }


    // Helper that visualize attacks ranges
    void OnDrawGizmosSelected ()
    {
        if (attackPoint != null) {
            Gizmos.DrawWireCube (attackPoint.position, attackRange);
        }
    }
}