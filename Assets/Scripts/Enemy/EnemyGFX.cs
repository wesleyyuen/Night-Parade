using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGFX : MonoBehaviour {
    private Transform player;
    private Enemy enemyScript;
    public float turningTime;
    private float aggroDistance;
    [HideInInspector] public bool isTurning;
    

    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        aggroDistance = gameObject.GetComponent<Enemy> ().aggroDistance;
    }
    void Update () {
        if (player == null) return;

        // Flip Sprite (using localScale instead of FlipX so children also flips, just in case...)
        if (Vector2.Distance (player.position, transform.position) < aggroDistance) {
            if (transform.position.x > player.position.x) {
                if (transform.localScale.x != -1f)
                    StartCoroutine (Turning (-1f, turningTime));
            } else {
                if (transform.localScale.x != 1f)
                    StartCoroutine (Turning (1f, turningTime));
            }
        }
    }

    IEnumerator Turning (float XScale, float turningTime) {
        isTurning = true;
        yield return new WaitForSeconds (turningTime);
        transform.localScale = new Vector3 (XScale, 1f, 1f);
        isTurning = false;
    }
}