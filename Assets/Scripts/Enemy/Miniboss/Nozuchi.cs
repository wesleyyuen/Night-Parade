using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nozuchi : Enemy {

    public override void Awake () {
        // Do not spawn if player already defeated it before
        bool destroyedBefore;
        FindObjectOfType<PlayerProgress> ().areaProgress.TryGetValue ("Nozuchi_Defeated", out destroyedBefore);
        if (destroyedBefore) Destroy (transform.parent.gameObject);

        base.Awake ();
    }
    public override IEnumerator Die () {
        // TODO: Dying Animation

        yield return new WaitForSeconds (dieTime);
        FindObjectOfType<BossTrigger> ().OpenExit (); // reopen exit
        GetComponent<EnemyDrop> ().SpawnDrops ();
        FindObjectOfType<PlayerProgress> ().areaProgress.Add ("Nozuchi_Defeated", true);
        Destroy (gameObject);
    }
}