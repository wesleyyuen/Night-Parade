using UnityEngine;

public class Parallax : MonoBehaviour {
    [SerializeField] private float multiplier = 0.0f;
    private enum ParallaxMode {
        Horizontal,
        Vertical,
        Omnidirectional
    }

    [SerializeField] private ParallaxMode parallaxMode;

    private Transform cameraTransform;

    private Vector3 startCameraPos;
    private Vector3 startPos;

    void Start () {
        cameraTransform = Camera.main.transform;
        startCameraPos = cameraTransform.position;
        startPos = transform.position;
    }

    private void LateUpdate () {
        var position = startPos;
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