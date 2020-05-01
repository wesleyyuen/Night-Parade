using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float movementSpeed;
    public float jumpForce;
    public Animator animator;
    public SpriteRenderer sr;
    public Rigidbody2D rb;
    public bool isGrounded = false;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0 && horizontalInput > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (horizontalInput != 0 && horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        animator.SetFloat("Horizontal", horizontalInput);

        Jump();

        Vector3 horizontalMovement = new Vector2(horizontalInput * movementSpeed, 0.0f);
        transform.position += horizontalMovement * Time.deltaTime;
        
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Impulse);
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
