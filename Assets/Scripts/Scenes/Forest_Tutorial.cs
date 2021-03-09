using TMPro;
using UnityEngine;

public class Forest_Tutorial : MonoBehaviour {
    [SerializeField] private Transform playerGroup;
    [SerializeField] private Transform area1SpawnPoint;
    [SerializeField] private TextMeshProUGUI forestText;
    [SerializeField] private float textShowingTime;
    [SerializeField] private float textFadingTime;

    void Start () {
        GameMaster.Instance.UpdateCurrentScene ();

        if (GameMaster.Instance.GetPrevScene () == "Forest_Area1") {
            foreach (Transform child in playerGroup) {
                child.position = area1SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }
        } else {
            StartCoroutine (Common.FadeText (forestText, textShowingTime, textFadingTime));
        }
    }
}