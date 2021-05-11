using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour {

    [Header ("References")]
    private PlayerPlatformCollision coll;
    private Rigidbody2D rb;
    private PlayerCombat combat;

    [Header ("Movement Settings")]
    [SerializeField] private float movementSpeed = 11f;
    public bool canWalk {get; private set;}
    public bool isHandicapped {get; set;}
    private float xRaw, yRaw;

    void Awake ()
    {
        coll = GetComponentInChildren<PlayerPlatformCollision>();
        rb = GetComponent<Rigidbody2D>();
        combat = GetComponent<PlayerCombat>();
        canWalk = true;
    }

    public void EnablePlayerMovement(bool enabled, float time = 0)
    {
        if (canWalk == enabled) return;

        if (time == 0)
            canWalk = enabled;
        else
            StartCoroutine(Common.ChangeVariableAfterDelay<bool>(e => canWalk = e, time, enabled, !enabled));
    }

    private void Update()
    {
        if (combat.isAttacking)
            canWalk = !coll.onGround;

        xRaw = Input.GetAxisRaw("Horizontal");
        yRaw = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate ()
    {
        if (!canWalk) return;

        // Move player
        Vector2 newVelocity;
        if (coll.onGround && coll.onSlope) {
            newVelocity = new Vector2 (-xRaw * movementSpeed * coll.slopeVector.x, -xRaw * movementSpeed * coll.slopeVector.y);
        } else { // !coll.onGround
            newVelocity = new Vector2 (xRaw * movementSpeed, rb.velocity.y);
        }   

        if (isHandicapped)
            rb.velocity = Vector2.Lerp(rb.velocity, newVelocity, Time.deltaTime * 0.1f);
        else 
            rb.velocity = newVelocity;
    }

    public IEnumerator HandicapMovement(float time)
    {
        isHandicapped = true;
        yield return new WaitForSeconds(time);
        isHandicapped = false;
    }

    public void StepForward(float dist, float time)
    {
        if (coll.onGround && xRaw != 0) {
            bool facingRight = transform.localScale.x > 0;
            rb.velocity = Vector2.zero;
            rb.MovePosition(rb.position + new Vector2((facingRight ? 1 : -1) * dist, 0));
        }
    }
}