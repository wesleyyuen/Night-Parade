using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private PlayerPlatformCollision grounded;
    [SerializeField] private ParticleSystem dustTrail;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement movement;
    private PlayerCombat combat;
    [HideInInspector] public bool canTurn;

    private void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        canTurn = true;
    }

    private void Update() {
        animator.SetBool ("IsGrounded", grounded.onGround);
        animator.SetFloat ("Vertical", rb.velocity.y);

        if (!canTurn) return;
        float horizontalInput = Input.GetAxisRaw ("Horizontal");
        Vector3 prevLocalScale = transform.localScale;

        // Flip sprite (TODO: maybe move into child object)
        if (horizontalInput > 0 && prevLocalScale.x != 1f) {
            //CreateDustTrail ();
            FaceRight(true);
        } else if (horizontalInput < 0 && prevLocalScale.x != -1f) {
            //CreateDustTrail ();
            FaceRight(false);
        }

        // Set animations
        if (movement.canWalk)
            animator.SetFloat ("Horizontal", horizontalInput);
    }

    public void FaceRight(bool faceRight) {
        animator.SetBool ("FacingRight", faceRight);
        
        transform.localScale = new Vector3 (faceRight ? 1f : -1f, 1f, 1f);
    } 

    public void SetTrigger(string trigger) {
        animator.SetTrigger(trigger);
    }

    public void CreateDustTrail () {
        if (grounded.onGround) {
            dustTrail.Play ();
        }
    }
}
