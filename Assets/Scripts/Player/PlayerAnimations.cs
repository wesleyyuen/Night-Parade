using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private PlayerPlatformCollision grounded;
    [SerializeField] private ParticleSystem dustTrail;

    private Animator animator;
    private Rigidbody2D rb;
    private void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        float horizontalInput = Input.GetAxisRaw ("Horizontal");
        Vector3 prevLocalScale = transform.localScale;

        // Flip sprite (TODO: maybe move into child object)
        if (horizontalInput > 0) {
            if (prevLocalScale.x != 1f) {
                CreateDustTrail ();
                animator.SetBool ("FacingRight", true);
                transform.localScale = new Vector3 (1f, 1f, 1f);
            }
        } else if (horizontalInput < 0) {
            if (prevLocalScale.x != -1f) {
                CreateDustTrail ();
                animator.SetBool ("FacingRight", false);
                transform.localScale = new Vector3 (-1f, 1f, 1f);
            }
        }

        // Set animation
        animator.SetBool ("IsGrounded", grounded.onGround);
        animator.SetFloat ("Horizontal", horizontalInput);
        animator.SetFloat ("Vertical", rb.velocity.y);
    }

    public void SetTrigger(string trigger) {
        animator.SetTrigger(trigger);
    }

    private void CreateDustTrail () {
        if (grounded.onGround) {
            dustTrail.Play ();
        }
    }
}
