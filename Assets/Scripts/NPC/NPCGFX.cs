using UnityEngine;

public class NPCGFX : MonoBehaviour {
    Transform player;

    protected virtual void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
    }
    protected virtual void Update () {
        if (player == null) return;
        if (transform.position.x > player.position.x) {
            transform.localScale = new Vector3 (-1f, 1f, 1f);
        } else if (transform.position.x < player.position.x) {
            transform.localScale = new Vector3 (1f, 1f, 1f);
        }
    }
}