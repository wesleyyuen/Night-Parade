using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScrolling : MonoBehaviour {

    [SerializeField] private float speed;
    private Vector3 originalPosition = new Vector3 (0f, 0f, 60f);
    private Vector3 wrappingPosition = new Vector3 (-195f, 0f, 60f);

    // TODO: would use Mathf.Repeat but can't figure out when does a group of cloud start/end/length
    // just pingpong the cloud for now instead
    void Update () {
        var dif = wrappingPosition - originalPosition;
        var t = Mathf.PingPong (Time.time * speed / dif.magnitude, 1f);
        transform.position = Vector3.Lerp (originalPosition, wrappingPosition, t);
    }
}