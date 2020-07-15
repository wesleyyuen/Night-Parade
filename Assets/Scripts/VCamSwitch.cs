using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VCamSwitch : MonoBehaviour {
    // Switching between Virtual Cameras

    [SerializeField] private GameObject vcam;

    void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag ("Player") && !other.isTrigger) {
            vcam.SetActive (true);
        }
    }

    void OnTriggerExit2D (Collider2D other) {
        if (other.CompareTag ("Player") && !other.isTrigger) {
            vcam.SetActive (false);
        }
    }
}