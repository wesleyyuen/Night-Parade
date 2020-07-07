using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hatsumura : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform area3SpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Area3") {
            player.position = area3SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        }
    }
}