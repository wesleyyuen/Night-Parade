using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerAnimations anim;
    private PlayerPlatformCollision collision;
    private PlayerMovement movement;
    [HideInInspector] public bool isDashing;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashTime;
    [SerializeField] private float cooldown;
    [SerializeField] private float freezeDuration;
    [SerializeField] private ParticleSystem afterimage;
    private float nextDashTime;
    private bool canDash;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<PlayerAnimations>();
        movement = GetComponentInParent<PlayerMovement>();
        Transform collTransform = transform.parent.Find("Platform Collision");
        canDash = true;
        if (collTransform)
            collision = collTransform.GetComponent<PlayerPlatformCollision>();
    }

    private void Update() {
        if (!canDash) return;

        float now = Time.time;
        if (Input.GetKeyDown(KeyCode.LeftShift) && now > nextDashTime) {
            nextDashTime = Time.time + cooldown;
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (dir == Vector2.zero)
                dir = -rb.transform.localScale.x * Vector2.left;
            // Dash(dir);
            StartCoroutine(StartDash(dir));
        }
    }

    private IEnumerator StartDash(Vector2 dir) {
        // Freeze position effect
        StartCoroutine( Common.ChangeVariableAfterDelay<RigidbodyConstraints2D>(e => rb.constraints = e, freezeDuration, RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation, RigidbodyConstraints2D.FreezeRotation));

        yield return new WaitForSeconds(freezeDuration);
        Dash(dir);
    }
    
    private void Dash (Vector2 dir) {
        isDashing = true;

        // Effects
        afterimage.GetComponent<ParticleSystemRenderer>().flip = new Vector3(dir.x > 0 ? 0f : 1f, 0f, 0f);
        afterimage.Play();

        StartCoroutine( movement.DisableMovement(dashTime));
        StartCoroutine( Common.ChangeVariableAfterDelay<float>(e => rb.gravityScale = e, dashTime, 0, rb.gravityScale));
        
        rb.velocity = Vector2.zero;
        rb.velocity += dir.normalized * dashSpeed;
    }

}
