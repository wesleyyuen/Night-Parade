using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTutorial : MonoBehaviour {

    [SerializeField] private GameObject textPrompt;

    void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag ("Player")) {
            textPrompt.SetActive(true);
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        if (other.CompareTag ("Player")) {
            textPrompt.SetActive(false);
        }
    }
}