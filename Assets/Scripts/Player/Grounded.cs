using UnityEngine;

public class Grounded : MonoBehaviour {

    GameObject player;
    void Start () {
        player = gameObject.transform.parent.gameObject;
    }

    private void OnTriggerStay2D (Collider2D collider) {
        if (collider.tag == "Ground") {
            player.GetComponent<PlayerMovement> ().isGrounded = true;
        }
    }

    private void OnTriggerExit2D (Collider2D collider) {
        player.GetComponent<PlayerMovement> ().isGrounded = false;
    }
}