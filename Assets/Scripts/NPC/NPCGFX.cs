using UnityEngine;

public class NPCGFX : MonoBehaviour {
    private Transform player;
    private SpriteRenderer spriteRenderer; // using flipX instead of localScale since textMesh is Children

    protected virtual void Start () {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected virtual void Update () {
        if (player == null) return;

        // Flip sprite to face player
        if (transform.position.x > player.position.x) {
            spriteRenderer.flipX = true;
        } else if (transform.position.x < player.position.x) {
            spriteRenderer.flipX = false;
        }
    }
}