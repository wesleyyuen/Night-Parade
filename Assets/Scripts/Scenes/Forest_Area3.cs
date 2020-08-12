using TMPro;
using UnityEngine;

public class Forest_Area3 : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform area2SpawnPoint;
    [SerializeField] private Transform area4SpawnPoint;
    [SerializeField] private Transform hatsumuraSpawnPoint;
    [SerializeField] private GameObject forestText;
    [SerializeField] private float textShowingTime;
    [SerializeField] private float textFadingTime;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Area2") {
            player.position = area2SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Forest_Area4") {
            player.position = area4SpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
        } else if (gameMaster.GetPrevScene () == "Hatsumura") {
            StartCoroutine (FindObjectOfType<Common> ().FadeText (forestText, textShowingTime, textFadingTime));
            player.position = hatsumuraSpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
        }
    }
}