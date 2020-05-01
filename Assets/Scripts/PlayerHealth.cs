using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currHealth {get; private set;}
    void Awake()
    {
        currHealth = FindObjectOfType<GameMaster>().savedPlayerHealth;
    }
    /*
    void OnCollisionEnter2D(Collider2D collider) {
        if (collider.CompareTag("Enemy")) {
            currHealth--;
        }
    }
    */
    void Update() {
        if (currHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
        Debug.Log("You died");
    }
}
