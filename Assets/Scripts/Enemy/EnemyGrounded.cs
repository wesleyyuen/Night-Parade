using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrounded : MonoBehaviour {

    public bool isGrounded;

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag ("Ground") && !isGrounded) {
            isGrounded = true;
        }
    }

    private void OnTriggerStay2D (Collider2D collider) { // needed to fix slope walking
        if (collider.tag == "Ground" && !isGrounded) {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D (Collider2D other) {
        isGrounded = false;
    }
}