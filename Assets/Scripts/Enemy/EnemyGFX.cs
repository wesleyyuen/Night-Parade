using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGFX : MonoBehaviour {
    private Transform player;
    public float turningTime;
    private EnemyAggression enemyAggression;
    [HideInInspector] private bool isTurning;
    [SerializeField] protected GameObject exclaimationMark;

    private void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        enemyAggression = GetComponent<EnemyAggression>();
    }

    public IEnumerator FaceTowardsPlayer (float delay) {
        if (player.position.x >= transform.position.x && transform.localScale.x != 1.0f) {
            yield return new WaitForSeconds (delay);
            transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
        } else if (player.position.x < transform.position.x && transform.localScale.x != -1.0f) {
            yield return new WaitForSeconds (delay);
            transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
        }
    }

    public IEnumerator PatrolTurnAround (bool isInstant) {
        isTurning = true;
        yield return new WaitForSeconds (isInstant ? 0.0f : turningTime);
        transform.localScale = new Vector3 (-transform.localScale.x, 1f, 1f);
        isTurning = false;
    }

    public IEnumerator FlashExclaimationMark () {
        exclaimationMark.SetActive (true);
        float flashTime = GetComponent<EnemyGFX> ().turningTime;
        if (flashTime < 0.5f) flashTime = 0.5f;
        yield return new WaitForSeconds (flashTime);
        exclaimationMark.SetActive (false);
    }

    public bool GetIsTurning() {
        return isTurning;
    }
}