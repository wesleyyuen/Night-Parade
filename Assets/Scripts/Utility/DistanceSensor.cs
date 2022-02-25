using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    private Camera m_camera;
    [SerializeField] float m_distanceThreshold = 70f;
    private int FIXED_UPDATE_INTERVAL = 10;

    private void Awake()
    {
        m_camera = Camera.main;
    }

    private void FixedUpdate()
    {
        // Only run every FIXED_UPDATE_INTERVAL frames
        if (Time.frameCount % FIXED_UPDATE_INTERVAL == 0) {
            foreach (Transform child in transform) {
                child.gameObject.SetActive(Vector2.Distance(child.position, m_camera.transform.position) <= m_distanceThreshold);
            }
        }
    }
}
