using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour {

    [SerializeField] private GameObject exitBlock;
    [SerializeField] private float bossExitShuttingTime;

    void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag ("Player")) {
            FindObjectOfType<Nozuchi> ().GetComponentInChildren<Animator> ().SetTrigger ("FightStarted");
            StartCoroutine (BossExitControl (-4f));
        }
    }

    public void OpenExit() {
        StartCoroutine (BossExitControl (0f));
    }

    IEnumerator BossExitControl (float yToMoveTo) {
        float y = exitBlock.transform.position.y;
        for (float t = 0f; t < 1f; t += Time.deltaTime / bossExitShuttingTime) {
            exitBlock.transform.position = new Vector3 (exitBlock.transform.position.x, Mathf.SmoothStep (y, yToMoveTo, t), exitBlock.transform.position.z);
            yield return null;
        }
    }
}