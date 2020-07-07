using Cinemachine;
using UnityEngine;

public class CameraPeeking : MonoBehaviour {
    private CinemachineFramingTransposer transposer;
    private bool isGrounded;
    [SerializeField] private float originalScreenY;
    [SerializeField] private float lookUpScreenY;
    [SerializeField] private float lookDownScreenY;
    [SerializeField] private float changeRate;
    private float startTime = 0f;
    private float timer = 0f;
    [SerializeField] private float holdTime = 1.0f; // how long you need to hold to trigger the effect

    void Start () {
        GameObject vcam = GameObject.FindGameObjectWithTag("MainVCam");
        Grounded grounded = FindObjectOfType<Grounded> ();
        if (vcam != null) transposer = vcam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer> ();
        if (grounded != null) isGrounded = grounded.isGrounded;
    }

    void ReInitializeVariables () {
        if (transposer == null) {
            GameObject vcam = GameObject.FindGameObjectWithTag("MainVCam");
            if (vcam != null) transposer = vcam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer> ();
        }
        Grounded grounded = FindObjectOfType<Grounded> ();
        if (grounded != null) isGrounded = grounded.isGrounded;
    }

    void Update () {
        ReInitializeVariables ();
        if (transposer == null || !isGrounded) return;
        float currScreenY = transposer.m_ScreenY;

        if ((Input.GetButtonDown ("Vertical") && isGrounded) || Input.GetButton("Attack")) {
            startTime = Time.time;
            timer = startTime;
        }

        if (Input.GetButton ("Vertical") && isGrounded && !Input.GetButton("Attack")) {
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