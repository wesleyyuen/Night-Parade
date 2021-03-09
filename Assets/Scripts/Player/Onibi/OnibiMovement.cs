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
                                         Constant.playerZOrder);
        desiredPoint = transform.position;
    }

    void FixedUpdate() {
        desiredPoint = new Vector2(player.position.x + (player.localScale.x == 1 ? -distanceFromPlayer : distanceFromPlayer),
                                   player.position.y + distanceFromPlayer / 2);
        rb.position = Vector2.SmoothDamp(rb.position, desiredPoint, ref refPosition, 0.3f, 15, Time.deltaTime);

        // Vector2 direction = (desiredPoint - (Vector2) transform.position);
        // if (direction.magnitude > 3) {
        //     float desiredAngle = Vector2.SignedAngle(Vector2.up, -direction.normalized);
        //     rb.rotation = Mathf.SmoothDampAngle(rb.rotation, desiredAngle, ref refRotation, 0.3f, 180, Time.deltaTime);
        // } else {
        //     rb.rotation = Mathf.SmoothDampAngle(rb.rotation, 0, ref refRotation, 0.3f, 180, Time.deltaTime);
        // }
    }
}
