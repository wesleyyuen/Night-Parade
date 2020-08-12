﻿using UnityEngine;

public class Nozuchi_Move : StateMachineBehaviour {

    private Transform player;
    private Rigidbody2D rb;
    private Nozuchi script;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float distanceToLunge;
    [SerializeField] private float lungeTendency;
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        rb = animator.GetComponentInParent<Rigidbody2D> ();
        script = animator.GetComponentInParent<Nozuchi>();
    }

    override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (player == null || script.isDead) return;

        // Move towards player position
        Vector2 target = new Vector2 (player.position.x, rb.position.y);
        Vector2 newPosition = Vector2.MoveTowards (rb.position, target, movementSpeed * Time.fixedDeltaTime);
        rb.position = newPosition;

        // if within range, have certain percentage to Lunge towards player
        if (Vector2.Distance (rb.position, player.position) <= distanceToLunge && Random.value < lungeTendency) {
            animator.SetTrigger ("Lunge");
        }
    }

    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.ResetTrigger ("Lunge");
    }
}