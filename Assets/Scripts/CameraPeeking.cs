using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraPeeking : MonoBehaviour {

    CinemachineFramingTransposer transposer;
    public float originalScreenY;
    public float lookUpScreenY;
    public float lookDownScreenY;
    public float changeRate;

    void Start () {
        transposer = FindObjectOfType<CinemachineVirtualCamera> ().GetCinemachineComponent<CinemachineFramingTransposer> ();
    }

    void Update () {
        float currScreenY = transposer.m_ScreenY;

        // Handle flipflopping
        if (!Input.GetButton ("Vertical") && currScreenY != originalScreenY && currScreenY > originalScreenY - 0.01 && currScreenY < originalScreenY + 0.01) {
            transposer.m_ScreenY = originalScreenY;
            return;
        }
        if (!Input.GetButton ("Vertical") && currScreenY != originalScreenY) {
            if (currScreenY > originalScreenY) transposer.m_ScreenY -= changeRate;
            else if (currScreenY < originalScreenY) transposer.m_ScreenY += changeRate;
        }
        if (Input.GetButton ("Vertical") && Input.GetAxis ("Vertical") > 0) {
            if (currScreenY < lookUpScreenY) transposer.m_ScreenY += changeRate;
            else if (currScreenY == lookUpScreenY) return;
        } else if (Input.GetButton ("Vertical") && Input.GetAxis ("Vertical") < 0) {

            if (currScreenY > lookDownScreenY) transposer.m_ScreenY -= changeRate;
            else if (currScreenY == lookDownScreenY) return;
        }
    }
}