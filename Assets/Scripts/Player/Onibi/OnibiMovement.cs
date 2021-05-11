using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnibiMovement : MonoBehaviour {
    [Header ("References")]
    private Transform player;
    private Rigidbody2D rb;
    [Header ("Parameters")]
    [SerializeField] private float distanceFromPlayer;
    private Vector2 refPosition;
    private float refRotation;
    private Vector2 desiredPoint;

    void Start() {
        player = transform.parent.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(player.position.x + (player.localScale.x == 1 ? -distanceFromPlayer : distanceFromPlayer),
                                         player.position.y + distanceFromPlayer / 2,
                                         0);
        desiredPoint = transform.position;

        // Overclock glow intensity
        Material mat = GetComponent<SpriteRenderer>().material;
        float factor = Mathf.Pow(2, 3);
        mat.color = new Color(mat.color.r * factor, mat.color.g * factor, mat.color.b * factor, mat.color.a);
    }

    void FixedUpdate() {
        desiredPoint = new Vector2(player.position.x + (player.localScale.x == 1 ? -distanceFromPlayer : distanceFromPlayer),
                                   player.position.y /*+ distanceFromPlayer / 2*/ + Mathf.Sin(Time.time * Random.value) * 0.3f);
        rb.position = Vector2.SmoothDamp(rb.position, desiredPoint, ref refPosition, 0.3f, 15, Time.deltaTime); 
    }
}
