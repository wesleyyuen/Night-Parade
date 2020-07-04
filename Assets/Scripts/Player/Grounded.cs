using UnityEngine;

public class Grounded : MonoBehaviour {

    AudioManager audioManager;
    public bool startOffGrounded;
    public bool isGrounded { get; private set; }
    public bool onSlope { get; private set; }

    void Awake () {
        audioManager = FindObjectOfType<AudioManager> ();
        if (startOffGrounded) isGrounded = true;
    }

    private void OnTriggerEnter2D (Collider2D collider) { // needed to play grounded audio
        if (collider.tag == "Ground" && !isGrounded) {
            audioManager.Play ("Forest_Landing");
            isGrounded = true;
        }
    }

    private void OnTriggerStay2D (Collider2D collider) {    // needed to fix slope walking
        if (collider.tag == "Ground" && !isGrounded) {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D (Collider2D collider) {
        isGrounded = false;
    }
}