using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    Camera _camera;
    [SerializeField] float distanceThreshold = 70f;

    void Start()
    {
        _camera = Camera.main;
    }

    void FixedUpdate()
    {
        // TODO: improve this
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(Vector2.Distance(child.position, _camera.transform.position) <= distanceThreshold);
        }
    }
}
