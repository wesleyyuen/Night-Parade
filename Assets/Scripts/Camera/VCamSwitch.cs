using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamSwitch : MonoBehaviour
{
    // Switching between Virtual Cameras

    [SerializeField] CinemachineVirtualCamera vcam;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
            vcam.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
            vcam.enabled = false;
    }
}