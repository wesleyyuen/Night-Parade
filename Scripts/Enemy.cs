using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 2;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage() {
        // TODO Animation
        currentHealth--;
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        // TODO Animation
        Debug.Log(gameObject.name + " Died");
       Destroy(gameObject);
    }
}
