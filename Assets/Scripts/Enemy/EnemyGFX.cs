using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGFX : MonoBehaviour {
    private Transform player;
    private Enemy enemyScript;
    public float turningTime;
    float aggroDistance;

    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        aggroDistance = gameObject.GetComponent<Enemy> ().aggroDistance;
    }
    void Update () {
        if (player == null) return;
        if (Vector2.Distance (player.position, transform.position) < aggroDistance) {
            if (transform.position.x > player.position.x) {
                StartCoroutine (Turning (-1f, turningTime));
            } else if (transform.position.x < player.position.x) {
                StartCoroutine (Turning (1f, turningTime));
            }
        }
    }

    IEnumerator Turning (float XScale, float turningTime) {
        yield return new WaitForSeconds (turningTime);
        transform.localScale = new Vector3 (XScale, 1f, 1f);
    }
}