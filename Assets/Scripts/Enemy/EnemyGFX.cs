using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGFX : MonoBehaviour {
    private Transform player;
    public float turningTime;
    private EnemyAggression enemyAggression;
    [HideInInspector] private bool isTurning;
    [SerializeField] protected GameObject exclaimationMark;
    [SerializeField] protected GameObject questionMark;

    private void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        enemyAggression = GetComponent<EnemyAggression>();
    }

    public IEnumerator FaceTowardsPlayer (float delay) {
        if (player.position.x >= transform.position.x && transform.localScale.x != 1.0f) {
            yield return new WaitForSeconds (delay);
            transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

            // Flip question mark back
            foreach (Transform child in transform) {
            if (child.CompareTag("Unflippable"))
                child.localScale = new Vector3 (Mathf.Abs(child.localScale.x), Mathf.Abs (child.localScale.y), 1.0f);
            }

        } else if (player.position.x < transform.position.x && transform.localScale.x != -1.0f) {
            yield return new WaitForSeconds (delay);
            transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
            
            // Flip question mark back
            foreach (Transform child in transform) {
            if (child.CompareTag("Unflippable"))
                child.localScale = new Vector3 (-Mathf.Abs(child.localScale.x), Mathf.Abs (child.localScale.y), 1.0f);
            }
        }
    }

    public IEnumerator PatrolTurnAround (bool isInstant) {
        isTurning = true;
        yield return new WaitForSeconds (isInstant ? 0.0f : turningTime);
        transform.localScale = new Vector3 (-transform.localScale.x, 1f, 1f);

        // Flip question mark back
        foreach (Transform child in transform) {
            if (child.CompareTag("Unflippable")) {
                //Debug.Log("reached");
                child.localScale = new Vector3 (-child.localScale.x , Mathf.Abs (child.localScale.y), 1.0f);
            }
                
        }
        isTurning = false;
    }

    public IEnumerator FlashExclaimationMark () {
        if (exclaimationMark == null)   
            yield return null;

        exclaimationMark.GetComponent<SpriteRenderer>().enabled = true;
        float flashTime = GetComponent<EnemyGFX> ().turningTime;

        // Flash at least 0.5 seconds
        if (flashTime < 0.5f) flashTime = 0.5f;
        yield return new WaitForSeconds (flashTime);
        exclaimationMark.GetComponent<SpriteRenderer>().enabled = false;
    }

    public IEnumerator FlashQuestionMark () {
        if (questionMark == null)
            yield return null;

        questionMark.GetComponent<SpriteRenderer>().enabled = true;
        float flashTime = GetComponent<EnemyGFX> ().turningTime;

        // Flash at least 0.5 seconds
        if (flashTime < 0.5f) flashTime = 0.5f;
        yield return new WaitForSeconds (flashTime);
        questionMark.GetComponent<SpriteRenderer>().enabled = false;
    }

    public bool GetIsTurning() {
        return isTurning;
    }
}