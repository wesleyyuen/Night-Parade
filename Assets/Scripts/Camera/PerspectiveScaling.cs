using UnityEngine;

public class PerspectiveScaling : MonoBehaviour
{  
    private void Awake()
    {
        if (transform.childCount == 0) {
            float scale = (transform.position.z - Camera.main.transform.position.z) / Camera.main.transform.position.z;
            transform.localScale = new Vector3(Mathf.Abs(scale), Mathf.Abs(scale), 1f);
        } else {
            foreach (Transform child in transform)
            {
                float scale = (child.position.z - Camera.main.transform.position.z) / Camera.main.transform.position.z;
                child.localScale = new Vector3(Mathf.Abs(scale), Mathf.Abs(scale), 1f);
            }
        }
    }
}
