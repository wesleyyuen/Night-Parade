using UnityEngine;

public class PlayerPlatformCollision : MonoBehaviour {  
    private AudioManager audioManager;
    [SerializeField] private bool startOffGrounded;
    [SerializeField] private float colliderRadius;
    public bool onGround { get; private set; }
    public bool onWall { get; private set; }
    public bool onLeftWall { get; private set; }
    public bool onRightWall { get; private set; }
    private Collider2D coll;
    private Vector2 groundDetectionPoint;
    private Vector2 leftDetectionPoint;
    private Vector2 rightDetectionPoint;

    private void Awake () {
        audioManager = FindObjectOfType<AudioManager> ();
        coll = GetComponentInParent<Collider2D>();

        if (startOffGrounded)
            onGround = true;
    }

    private void Update() {
        groundDetectionPoint = new Vector2 (coll.bounds.center.x, coll.bounds.min.y);
        leftDetectionPoint = new Vector2 (coll.bounds.min.x, coll.bounds.center.y);
        rightDetectionPoint = new Vector2 (coll.bounds.max.x, coll.bounds.center.y);

        onGround = Physics2D.OverlapCircle(groundDetectionPoint, colliderRadius, LayerMask.GetMask("Ground"));
        onLeftWall = Physics2D.OverlapCircle(leftDetectionPoint, colliderRadius, LayerMask.GetMask("Wall"));
        onRightWall = Physics2D.OverlapCircle(rightDetectionPoint, colliderRadius, LayerMask.GetMask("Wall"));
        onWall = onLeftWall || onRightWall;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(groundDetectionPoint, colliderRadius);
        Gizmos.DrawWireSphere(leftDetectionPoint, colliderRadius);
        Gizmos.DrawWireSphere(rightDetectionPoint, colliderRadius);
    }
}