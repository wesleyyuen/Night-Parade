using Cinemachine;
using UnityEngine;

public class CameraPeeking : MonoBehaviour
{
    CinemachineFramingTransposer transposer;
    bool isGrounded;
    [SerializeField] float originalScreenY;
    [SerializeField] float lookUpScreenY;
    [SerializeField] float lookDownScreenY;
    [SerializeField] float changeRate;
    float startTime = 0f;
    float timer = 0f;
    [SerializeField] float holdTime = 1.0f; // how long you need to hold to trigger the effect

    private void Start ()
    {
        this.enabled = false;

        GameObject vcam = GameObject.FindGameObjectWithTag ("MainVCam");
        PlayerPlatformCollision grounded = FindObjectOfType<PlayerPlatformCollision> ();
        if (vcam != null) transposer = vcam.GetComponent<CinemachineVirtualCamera> ().GetCinemachineComponent<CinemachineFramingTransposer> ();
        if (grounded != null) isGrounded = grounded.onGround;
    }

    private void ReInitializeVariables ()
    {
        if (transposer == null) {
            GameObject vcam = GameObject.FindGameObjectWithTag ("MainVCam");
            if (vcam != null) transposer = vcam.GetComponent<CinemachineVirtualCamera> ().GetCinemachineComponent<CinemachineFramingTransposer> ();
        }
        PlayerPlatformCollision grounded = FindObjectOfType<PlayerPlatformCollision> ();
        if (grounded != null) isGrounded = grounded.onGround;
    }

    private void Update ()
    {
        ReInitializeVariables ();
        if (transposer == null || !isGrounded) return;
        float currScreenY = transposer.m_ScreenY;

        if ((Input.GetButtonDown ("Vertical") && isGrounded) || Input.GetButton ("Attack")) {
            startTime = Time.time;
            timer = startTime;
        }

        // Start moving Camera
        if (Input.GetButton ("Vertical") && isGrounded && !Input.GetButton ("Attack")) {
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