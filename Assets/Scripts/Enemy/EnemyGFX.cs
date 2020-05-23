using UnityEngine;

public class EnemyGFX : MonoBehaviour {
    Transform player;
    float aggroDistance;

    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        aggroDistance = GetComponent<Enemy> ().aggroDistance;
    }
    void Update () {
        if (player == null) return;
        if (Vector2.Distance (player.position, transform.position) < aggroDistance) {
            if (transform.position.x > player.position.x) {
                transform.localScale = new Vector3 (-1f, 1f, 1f);
            } else if (transform.position.x < player.position.x) {
                transform.localScale = new Vector3 (1f, 1f, 1f);
            }
        }
    }
}