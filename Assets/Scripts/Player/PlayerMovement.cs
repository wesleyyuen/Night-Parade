using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour {

    [Header ("References")]
    private PlayerPlatformCollision coll;
    private Rigidbody2D rb;

    [Header ("Movement Settings")]
    [SerializeField] private float movementSpeed = 11f;
    private float horizontalInput;
    public bool canWalk {get; set;}
    public bool isHandicapped {get; set;}

    void Awake () {
        coll = GetComponentInChildren<PlayerPlatformCollision>();
        rb = GetComponent<Rigidbody2D>();
        canWalk = true;
    }

    void FixedUpdate () {
        if (!canWalk)
           return;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

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

    public IEnumerator DisableMovement(float time) {
        canWalk = false;
        yield return new WaitForSeconds(time);
        canWalk = true;
    }

    public IEnumerator HandicapMovement(float time) {
        isHandicapped = true;
        yield return new WaitForSeconds(time);
        isHandicapped = false;
    }
}