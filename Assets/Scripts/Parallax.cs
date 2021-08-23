using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float multiplier = 0.0f;
    enum ParallaxMode {
        Horizontal,
        Vertical,
        Omnidirectional
    }

    [SerializeField] ParallaxMode parallaxMode;

    Transform cameraTransform;

    Vector3 startCameraPos;
    Vector3 startPos;

    void Start ()
    {
        cameraTransform = Camera.main.transform;
        startCameraPos = cameraTransform.position;
        startPos = transform.position;
    }

    void LateUpdate ()
    {
        var position = startPos;
        // Move background according to mode
        if (parallaxMode == ParallaxMode.Horizontal) {
            position.x += multiplier * (cameraTransform.position.x - startCameraPos.x);
        } else if (parallaxMode == ParallaxMode.Vertical) {
            position.y += multiplier * (cameraTransform.position.y - startCameraPos.y);
        } else {
            position += multiplier * (cameraTransform.position - startCameraPos);
        }

        transform.position = position;
    }
}