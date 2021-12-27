using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    Camera m_camera;
    [SerializeField] float m_distanceThreshold = 70f;

    private void Start()
    {
        m_camera = Camera.main;
    }

    private void FixedUpdate()
    {
        // TODO: improve this
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(Vector2.Distance(child.position, m_camera.transform.position) <= m_distanceThreshold);
        }
    }
}
