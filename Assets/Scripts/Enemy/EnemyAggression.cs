using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAggression : MonoBehaviour {
    [SerializeField] protected float aggroRange;

    [Header ("Line Of Sight")]
    [SerializeField] protected float lineOfSightDistance;
    [SerializeField] protected float lineOfSightAngle;
    [SerializeField] protected Vector2 lineOfSightOriginOffset;
    [SerializeField] protected float lineOfSightBreakDelay; // Amount of time remaining in aggro after LOS broken

    protected float delayTimer;
    protected Collider2D player;
    protected Rigidbody2D rb;
    protected Enemy enemy;
    protected EnemyGFX enemyGFX;
    protected bool isAggro;

    protected virtual void Start () {
        enemy = GetComponent<Enemy> ();
        enemyGFX = GetComponent<EnemyGFX> ();
        player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D> ();
        lineOfSightAngle /= 2;
        delayTimer = 0;
        isAggro = false;
    }

    protected virtual void Update () {
        if (player == null) return;

        // check Line of Sight
        bool inLineOfSight = InLineOfSight();
        if (!isAggro) {
            if (InAggroRange() || inLineOfSight) {
                isAggro = true;
                StartCoroutine (enemyGFX.FlashExclaimationMark ());
                delayTimer = 0;
            }
        } else {
            // Always face player when aggro
            StartCoroutine(enemyGFX.FaceTowardsPlayer(0.0f));

            // Drop aggro after a certain delay period without LOS
            if (!inLineOfSight) {
                delayTimer += Time.deltaTime;

                if (delayTimer >= lineOfSightBreakDelay) {
                    StartCoroutine (enemyGFX.FlashQuestionMark ());
                    isAggro = false;
                    delayTimer = 0;
                }
            } else {
                delayTimer = 0;
            }
        }
    }

    protected virtual void FixedUpdate () {}

    protected bool InLineOfSight () {
        // Check Distance to player
        Vector3 lineOfSightOrigin = transform.position + (Vector3) lineOfSightOriginOffset;
        Vector2 enemyToPlayerVector = player.bounds.center - lineOfSightOrigin;
        if (enemyToPlayerVector.magnitude > lineOfSightDistance) return false;

        // Check Line of sight angle
        float angle = Vector2.Angle(enemyToPlayerVector, transform.localScale.x * transform.right);
        if (angle > lineOfSightAngle) return false;

        // Check Line of sight with raycast2d
        int layerMasks = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D los = Physics2D.Raycast(lineOfSightOrigin, enemyToPlayerVector, lineOfSightDistance, layerMasks);
        if (los) return los.collider.CompareTag("Player");

        return false;
    }

    protected bool InAggroRange() {
        // Check Distance to player
        Vector3 lineOfSightOrigin = transform.position + (Vector3) lineOfSightOriginOffset;
        Vector2 enemyToPlayerVector = player.bounds.center - lineOfSightOrigin;
        if (enemyToPlayerVector.magnitude > aggroRange) return false;

        // Check Line of sight with raycast2d
        int layerMasks = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D los = Physics2D.Raycast(lineOfSightOrigin, enemyToPlayerVector, lineOfSightDistance, layerMasks);
        if (los) return los.collider.CompareTag("Player");

        return false;
    }

    // Remain aggro for some time after breaking LOS, then checking LOS again
    protected IEnumerator AggroDelayAfterBrokenLOS (float time) {
        yield return new WaitForSeconds (time);
        if (!InLineOfSight()) {
            isAggro = false;
        }
    }

    public bool GetIsAggro() {
        return isAggro;
    }
}