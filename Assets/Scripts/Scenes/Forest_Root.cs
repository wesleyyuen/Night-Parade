using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Root : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform treeAreaSpawnPoint;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();

        if (gameMaster.GetPrevScene () == "Forest_Tree") {
            player.position = treeAreaSpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        }
    }
}