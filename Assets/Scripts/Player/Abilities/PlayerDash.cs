using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/*
Modified from MixAndJam - Celeste's Movement
*/
public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimations anim;
    private PlayerAbilityController abilityController;
    private PlayerPlatformCollision collision;
    private PlayerMovement movement;
    [HideInInspector] public bool isDashing;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float cooldown;
    private float nextDashTime;
    private bool canDash;
    private void Awake() {
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<PlayerAnimations>();
        abilityController = GetComponentInParent<PlayerAbilityController>();
        movement = GetComponentInParent<PlayerMovement>();
        Transform collTransform = transform.parent.Find("Platform Collision");
        if (collTransform)
            collision = collTransform.GetComponent<PlayerPlatformCollision>();
    }

    private void Update() {
        float now = Time.time;
        if (Input.GetKeyDown(KeyCode.LeftShift) && now > nextDashTime) {
            nextDashTime = Time.time + cooldown;
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (dir == Vector2.zero)
                dir = -rb.transform.localScale.x * Vector2.left;
            Dash(dir);
        }
    }
    
    private void Dash (Vector2 dir) {
        isDashing = true;
        canDash = false;

        StartCoroutine( movement.DisableMovement(dashTime));
        StartCoroutine( Common.ChangeVariableAfterDelay<float>(e => rb.gravityScale = e, dashTime, 0, rb.gravityScale));
        
        rb.velocity = Vector2.zero;
        rb.velocity += dir.normalized * dashSpeed;
    }

}
