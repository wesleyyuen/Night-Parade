using UnityEngine;

public class Nozuchi_Move : StateMachineBehaviour {

    Transform player;
    Rigidbody2D rb;
    public float movementSpeed;
    public float distanceToLunge;
    public float lungeTendency;
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = animator.GetComponentInParent<Rigidbody2D> ();
    }

    override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (player == null) return;

        Vector2 target = new Vector2 (player.position.x, rb.position.y);
        Vector2 newPosition = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.fixedDeltaTime);
        rb.position = newPosition;

        if (Vector2.Distance (rb.position, player.position) <= distanceToLunge && Random.value > (1 - lungeTendency)) {
            animator.SetTrigger ("Lunge");
        }
    }

    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger ("Lunge");
    }
}