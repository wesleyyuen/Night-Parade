using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [SerializeField] protected float aggroDistance;
    [SerializeField] protected float movementSpeed;
    protected Transform player;
    protected Rigidbody2D rb;
    protected Enemy enemy;
    protected EnemyGFX enemyGFX;
    protected bool isAggro;
    [SerializeField] protected GameObject exclaimationMark;

    protected virtual void Start () {
        enemy = GetComponent<Enemy> ();
        enemyGFX = GetComponent<EnemyGFX> ();
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = GetComponent<Rigidbody2D> ();
        isAggro = false;
    }

    protected virtual void Update () {
        if (player == null) return;

        // Move towards player if not freezing after collision
        if (Vector2.Distance (player.position, rb.position) < aggroDistance) {
            // Flash Exclaimation Mark if enemy first spots the player
            if (!isAggro) {
                isAggro = true;
                StartCoroutine (FlashExclaimationMark ());
                return;
            }
            // Only move after enemy is done turning
            if (!enemyGFX.isTurning && !enemy.collisionOnCooldown) {
                Vector2 target = new Vector2 (player.position.x, rb.position.y);
                Vector2 newPosition = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.deltaTime);
                rb.position = newPosition;
            }
        } else {
            isAggro = false;
        }
    }

    protected IEnumerator FlashExclaimationMark () {
        exclaimationMark.SetActive (true);
        float flashTime = GetComponent<EnemyGFX> ().turningTime;
        if (flashTime < 0.5f) flashTime = 0.5f;
        yield return new WaitForSeconds (flashTime);
        exclaimationMark.SetActive (false);
    }
}