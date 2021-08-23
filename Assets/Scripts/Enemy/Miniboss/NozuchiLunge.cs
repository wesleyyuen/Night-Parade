using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NozuchiLunge : MonoBehaviour
{
    [SerializeField] float lungingForce;
    Rigidbody2D rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody2D> ();
    }

    // Lunge forward
    public void Lunge ()
    {
        rb.AddForce (new Vector2 (transform.localScale.x * lungingForce, 1f), ForceMode2D.Impulse);
    }
}