using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour {
    private bool isColliding = false;
    private void OnCollisionEnter2D (Collision2D collision) {
        if (isColliding) return;
        isColliding = true;
        if (collision.gameObject.layer == LayerMask.NameToLayer ("Enemies")) {
            GetComponent<PlayerHealth> ().TakeDamage (25f);
        }
    }

    private void OnCollisionExit2D (Collision2D collision) {
        if (isColliding) isColliding = false;
    }
}