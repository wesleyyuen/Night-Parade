using TMPro;
using UnityEngine;

public class Forest_Area3 : MonoBehaviour {
    [SerializeField] private Transform playerGroup;
    [SerializeField] private Transform area2SpawnPoint;
    [SerializeField] private Transform area4SpawnPoint;
    [SerializeField] private Transform hatsumuraSpawnPoint;
    [SerializeField] private TextMeshProUGUI forestText;
    [SerializeField] private float textShowingTime;
    [SerializeField] private float textFadingTime;
    void Start () {
        GameMaster.Instance.UpdateCurrentScene ();

        if (GameMaster.Instance.GetPrevScene () == "Forest_Area2") {
            foreach (Transform child in playerGroup) {
                child.position = area2SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
        } else if (GameMaster.Instance.GetPrevScene () == "Forest_Area4") {
            foreach (Transform child in playerGroup) {
                child.position = area4SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }            
        } else if (GameMaster.Instance.GetPrevScene () == "Hatsumura") {
            StartCoroutine (Common.FadeText (forestText, textShowingTime, textFadingTime));
            foreach (Transform child in playerGroup) {
                child.position = hatsumuraSpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }                        
        }
    }
}