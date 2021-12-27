using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nozuchi : MonoBehaviour
{
    protected GameObject player;
    protected Rigidbody2D rb;
    protected EnemyFSM fsm;
    Animator animator;
    EnemyGFX enemyGFX;
    [SerializeField] float attackTime;
    [HideInInspector] public bool collisionOnCooldown;
    public bool isDead { private set; get; }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        fsm = GetComponent<EnemyFSM>();
        collisionOnCooldown = false;
        isDead = false;

        // Do not spawn if player already defeated it before
        if (SaveManager.Instance.HasOverallProgress("Nozuchi_Defeated")) {
            Destroy(transform.parent.gameObject);
            return;
        }

        // base.Start ();
        animator = GetComponent<Animator>();
        enemyGFX = GetComponent<EnemyGFX>();
    }

    private void Update()
    {
        // Only face player if it is not attacking
        if (animator.GetBool("FightStarted"))
            enemyGFX.FaceTowardsPlayer(attackTime);

        // base.Update();
    }

    IEnumerator Die()
    {
        // Ignore Player Collision to avoid player taking dmg when running into dying enemy
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
        GetComponent<Animator>().enabled = false;

        GetComponent<SpriteFlash>().PlayDeathFlashEffect(fsm.enemyData.dieTime);
        yield return new WaitForSeconds(fsm.enemyData.dieTime);

        FindObjectOfType<BossTrigger>().OpenExit(); // reopen exit
        GetComponent<EnemyDrop>().SpawnDrops();
        SaveManager.Instance.AddOverallProgress("Nozuchi_Defeated", 1);
        Destroy(gameObject);
    }
}