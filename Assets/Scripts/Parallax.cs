using UnityEngine;

public class Parallax : MonoBehaviour {
    public enum ParallaxDirection {
        vertical,
        horizontal,
        verticalAndHorizontal
    }
    public ParallaxDirection direction;

    public Transform[] backgrounds;
    private float[] scales;
    public float smoothness = 1f;

    Transform cam;
    Vector3 previousCameraPosition;

    void Awake () {
        cam = Camera.main.transform;
    }

    void Start () {
        previousCameraPosition = cam.position;
        scales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++) {
            scales[i] = backgrounds[i].position.z * -1;
        }
    }

    public void FixParallaxOnLoad () {
        // TODO Move Background By Offset when loading new scene
        // current by hardcode moving the background in Scene scripts
    }

    void Update () {
        if (direction == ParallaxDirection.horizontal) {
            for (int i = 0; i < backgrounds.Length; i++) {
                float parralaxX = (previousCameraPosition.x - cam.position.x) * scales[i];
                float backgroundTargetPosX = backgrounds[i].position.x + parralaxX;
                Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
                backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothness * Time.deltaTime);
            }
        } else if (direction == ParallaxDirection.vertical) {
            for (int i = 0; i < backgrounds.Length; i++) {
                float parralaxY = (previousCameraPosition.y - cam.position.y) * scales[i];
                float backgroundTargetPosY = backgrounds[i].position.y + parralaxY;
                Vector3 backgroundTargetPos = new Vector3 (backgrounds[i].position.x, backgroundTargetPosY, backgrounds[i].position.z);
                backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothness * Time.deltaTime);
            }
        } else if (direction == ParallaxDirection.verticalAndHorizontal) {
            for (int i = 0; i < backgrounds.Length; i++) {
                float parralaxX = (previousCameraPosition.x - cam.position.x) * scales[i];
                float backgroundTargetPosX = backgrounds[i].position.x + parralaxX;
                float parralaxY = (previousCameraPosition.y - cam.position.y) * scales[i];
                float backgroundTargetPosY = backgrounds[i].position.y + parralaxY;
                Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);
                backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothness * Time.deltaTime);
            }
        }
        previousCameraPosition = cam.position;
    }
}