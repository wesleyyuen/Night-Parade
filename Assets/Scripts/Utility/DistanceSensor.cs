using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSensor : MonoBehaviour
{
    Camera cam;
    [SerializeField] float distanceThreshold = 70f;

    void Start()
    {
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(Vector2.Distance(child.position, cam.transform.position) <= distanceThreshold);
        }
    }
}
