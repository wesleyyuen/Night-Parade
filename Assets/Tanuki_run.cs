using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanuki_run : StateMachineBehaviour {
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    Transform player;
    Rigidbody2D rb;
    public float movementSpeed = 2.5f;
    public float DistanceToAttack = 7f;
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = animator.GetComponent<Rigidbody2D> ();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (player == null) return;

        Vector2 target = new Vector2 (player.position.x, rb.position.y);
        Vector2 newPosition = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.fixedDeltaTime);
        rb.position = newPosition;

        if (Vector2.Distance (rb.position, player.position) <= DistanceToAttack) {
            animator.SetTrigger ("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger ("Attack");
    }
}