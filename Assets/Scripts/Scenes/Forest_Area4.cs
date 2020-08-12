using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Forest_Area4 : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform area2SpawnPoint;
    [SerializeField] private PlayableDirector area2SpawnAnimation;
    [SerializeField] private Transform area3SpawnPoint;
    [SerializeField] private PlayableDirector area3SpawnAnimation;
    [SerializeField] private Transform area7SpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Area2") {
            player.position = area2SpawnPoint.position;
            player.localScale = new Vector3 (-1f, 1f, 1f);
            area2SpawnAnimation.Play ();
        } else if (gameMaster.GetPrevScene () == "Forest_Area3") {
            player.position = area3SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
            area3SpawnAnimation.Play ();
        } else if (gameMaster.GetPrevScene () == "Forest_Area7") {
            player.position = area7SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        }
    }
}