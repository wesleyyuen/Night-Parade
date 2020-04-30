using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour
{
    public float slowerSlowDownRate = 3f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Slower")
        {
            gameObject.GetComponent<PlayerMovement>().movementSpeed /= slowerSlowDownRate;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Slower")
        {
            gameObject.GetComponent<PlayerMovement>().movementSpeed *= slowerSlowDownRate;
        }
    }
}