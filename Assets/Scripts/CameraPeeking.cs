using Cinemachine;
using UnityEngine;

public class CameraPeeking : MonoBehaviour {
    CinemachineFramingTransposer transposer;
    PlayerMovement playerMovement;
    public float originalScreenY;
    public float lookUpScreenY;
    public float lookDownScreenY;
    public float changeRate;
    private float startTime = 0f;
    private float timer = 0f;
    public float holdTime = 1.0f; // how long you need to hold to trigger the effect

    void Start () {
        CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera> ();
        if (vcam != null) transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer> ();
        playerMovement = FindObjectOfType<PlayerMovement> ();
    }

    void ReInitializeVariables () {
        if (transposer == null) {
            CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera> ();
            if (vcam != null) transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer> ();
        }
        if (playerMovement == null) playerMovement = FindObjectOfType<PlayerMovement> ();
    }

    void Update () {
        ReInitializeVariables ();
        if (transposer == null || playerMovement == null || !playerMovement.isGrounded) return;
        float currScreenY = transposer.m_ScreenY;

        if (Input.GetButtonDown ("Vertical")) {
            startTime = Time.time;
            timer = startTime;
        }

        if (Input.GetButton ("Vertical")) {
            timer += Time.deltaTime;
            if (timer > startTime + holdTime) {
                if (Input.GetAxis ("Vertical") > 0) {
                    if (currScreenY < lookUpScreenY) transposer.m_ScreenY += changeRate;
                    else if (currScreenY == lookUpScreenY) return;
                } else if (Input.GetAxis ("Vertical") < 0) {
                    if (currScreenY > lookDownScreenY) transposer.m_ScreenY -= changeRate;
                    else if (currScreenY == lookDownScreenY) return;
                }
            }
        }

        // Handle flipflopping
        if (!Input.GetButton ("Vertical") && currScreenY != originalScreenY && currScreenY > originalScreenY - 0.01 && currScreenY < originalScreenY + 0.01) {
            transposer.m_ScreenY = originalScreenY;
            return;
        }
        if (!Input.GetButton ("Vertical") && currScreenY != originalScreenY) {
            if (currScreenY > originalScreenY) transposer.m_ScreenY -= changeRate;
            else if (currScreenY < originalScreenY) transposer.m_ScreenY += changeRate;
        }
    }
}