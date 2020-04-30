using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerLoadLevel : MonoBehaviour
{
    [SerializeField] private string levelToLoad;

    void OnTriggerEnter2D(Collider2D collider) {

        if (collider.CompareTag("Player")) {
            FindObjectOfType<GameMaster>().requestSceneChange(levelToLoad, FindObjectOfType<PlayerHealth>().currHealth);
        }
    }
}
