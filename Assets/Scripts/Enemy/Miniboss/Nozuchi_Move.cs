using UnityEngine;

public class Nozuchi_Move : StateMachineBehaviour
{
    Transform player;
    Transform transform;
    Rigidbody2D rb;
    Nozuchi script;
    [SerializeField] float movementSpeed;
    [SerializeField] float distanceToLunge;
    [SerializeField] float lungeTendency;
    bool flag;
    
    public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        transform = animator.GetComponent<Transform>();
        rb = animator.GetComponentInParent<Rigidbody2D> ();
        script = animator.GetComponentInParent<Nozuchi>();
    }

    public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null || script.isDead) return;

        // Move towards player position
        Vector2 target = new Vector2 (player.position.x, rb.position.y);
        Vector2 direction = (target - (Vector2) transform.position).normalized;
        rb.MovePosition((Vector2) transform.position + direction * movementSpeed * Time.fixedDeltaTime);

        // if within range, have certain percentage to Lunge towards player
        if (Vector2.Distance (rb.position, player.position) <= distanceToLunge && Random.value < lungeTendency)
        {
            animator.SetTrigger ("Lunge");
        }
    }

    public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger ("Lunge");
    }
}