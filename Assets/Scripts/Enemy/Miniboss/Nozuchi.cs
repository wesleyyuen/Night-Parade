using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nozuchi : Enemy {
    private Animator animator;
    private EnemyGFX enemyGFX;
    [SerializeField] private float attackTime;

    public override void Start () {
        // Do not spawn if player already defeated it before
        bool defeatedBefore;
        FindObjectOfType<PlayerProgress> ().areaProgress.TryGetValue ("Nozuchi_Defeated", out defeatedBefore);
        if (defeatedBefore) Destroy (transform.parent.gameObject);

        base.Start ();
        animator = GetComponent<Animator>();
        enemyGFX = GetComponent<EnemyGFX>();
    }

    public override void Update () {
        // Only face player if it is not attacking
        if (animator.GetBool("FightStarted")) {
            StartCoroutine(enemyGFX.FaceTowardsPlayer(attackTime));
        }

        base.Update();
    }

    public override IEnumerator Die () {
        // Ignore Player Collision to avoid player taking dmg when running into dying enemy
        Physics2D.IgnoreCollision (player.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
        GetComponent<Animator>().enabled = false;

        GetComponent<SpriteFlash>().PlayDeathFlashEffect(dieTime);
        yield return new WaitForSeconds(dieTime);

        FindObjectOfType<BossTrigger> ().OpenExit (); // reopen exit
        GetComponent<EnemyDrop> ().SpawnDrops ();
        FindObjectOfType<PlayerProgress> ().areaProgress.Add ("Nozuchi_Defeated", true);
        Destroy (gameObject);
    }
}