using UnityEngine;

public class EnemyGFX : MonoBehaviour {
    Transform player;

    void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
    }
    void Update () {
        if (player == null) return;
        if (transform.position.x > player.position.x) {
            transform.localScale = new Vector3 (-1f, 1f, 1f);
        } else if (transform.position.x < player.position.x) {
            transform.localScale = new Vector3 (1f, 1f, 1f);
        }
    }
}