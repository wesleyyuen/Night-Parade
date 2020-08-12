using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nozuchi : Enemy {

    public override void Start () {
        // Do not spawn if player already defeated it before
        bool destroyedBefore;
        FindObjectOfType<PlayerProgress> ().areaProgress.TryGetValue ("Nozuchi_Defeated", out destroyedBefore);
        if (destroyedBefore) Destroy (transform.parent.gameObject);

        base.Start ();
    }

    public override IEnumerator Die () {
        // Ignore Player Collision to avoid player taking dmg when running into dying enemy
        Physics2D.IgnoreCollision (player.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
        GetComponent<Animator>().enabled = false;

        // Dying Animation
        for (float t = 0f; t < 1f; t += Time.deltaTime / dieTime) {
            sr.color = new Color (Mathf.Lerp (1, 0, t), Mathf.Lerp (1, 0, t), Mathf.Lerp (1, 0, t), 1.0f);
            yield return null;
        }

        //yield return new WaitForSeconds (dieTime);
        FindObjectOfType<BossTrigger> ().OpenExit (); // reopen exit
        GetComponent<EnemyDrop> ().SpawnDrops ();
        FindObjectOfType<PlayerProgress> ().areaProgress.Add ("Nozuchi_Defeated", true);
        Destroy (gameObject);
    }
}