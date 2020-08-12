using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour {

    [SerializeField] private GameObject exitBlock;
    [SerializeField] private float bossExitShuttingTime;
    [SerializeField] private Vector2 bossExitShuttingDirection;
    [SerializeField] private Animator animator;

    void OnTriggerExit2D (Collider2D other) {
        if (other.CompareTag ("Player")) {
            // Start Boss Engagement
            animator.SetTrigger ("FightStarted");
            // Closing Boss Exit
            StartCoroutine (BossExitControl (bossExitShuttingDirection));
        }
    }

    // Allow it to be called when boss dies
    public void OpenExit () {
        StartCoroutine (BossExitControl (new Vector2 (0f, 0f)));
    }

    // Coroutine to open or close exit based on direction given
    IEnumerator BossExitControl (Vector2 direction) {
        float x = exitBlock.transform.position.x;
        float y = exitBlock.transform.position.y;
        for (float t = 0f; t < 1f; t += Time.deltaTime / bossExitShuttingTime) {
            exitBlock.transform.position = new Vector3 (Mathf.SmoothStep (x, direction.x, t), Mathf.SmoothStep (y, direction.y, t), exitBlock.transform.position.z);
            yield return null;
        }
    }
}