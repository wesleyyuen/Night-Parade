using UnityEngine;
using TMPro;
using MEC;

public class Forest_Tutorial : MonoBehaviour {
    [SerializeField] private Transform playerGroup;
    [SerializeField] private Transform area1SpawnPoint;
    [SerializeField] private TextMeshProUGUI forestText;
    [SerializeField] private float textShowingTime;
    [SerializeField] private float textFadingTime;

    void Start() {
        if (GameMaster.Instance.prevScene == "Forest_Area1") {
            foreach (Transform child in playerGroup) {
                child.position = area1SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }
        } else {
            Timing.RunCoroutine(Utility._FadeTextInAndOut (forestText, textShowingTime, textFadingTime));
        }
    }
}