using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour {

    [Header ("References")]
    private AudioManager audioManager;
    private Rigidbody2D rb;

    [Header ("Movement Settings")]
    [SerializeField] private float movementSpeed = 11f;
    private float horizontalInput;
    public bool canWalk {get; private set;}

    void Awake () {
        audioManager = FindObjectOfType<AudioManager> ();
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
        rb.velocity = new Vector2 (xRaw * movementSpeed, rb.velocity.y);
    }

    public IEnumerator DisableMovement(float time) {
        canWalk = false;
        yield return new WaitForSeconds(time);
        canWalk = true;
    }

    // public IEnumerator DisableGravityScale(float time) {
    //     float original = rb.gravityScale;
    //     rb.gravityScale = 0;
    //     yield return new WaitForSeconds(time);
    //     rb.gravityScale = original;
    // }

    void FootstepSFX () {
        audioManager.Play ("Forest_Footsteps");
    }
}