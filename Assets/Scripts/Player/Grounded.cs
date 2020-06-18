using UnityEngine;

public class Grounded : MonoBehaviour {

    public bool isGrounded { get; private set; }

    private void OnTriggerStay2D (Collider2D collider) {
        if (collider.tag == "Ground") {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D (Collider2D collider) {
        isGrounded = false;
    }
}