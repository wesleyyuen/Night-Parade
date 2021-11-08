using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest_Root : MonoBehaviour {
    [SerializeField] private Transform playerGroup;
    [SerializeField] private Transform treeAreaSpawnPoint;
    void Start () {

        if (GameMaster.Instance.prevScene == "Forest_Tree") {
            foreach (Transform child in playerGroup) {
                child.position = treeAreaSpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
        }
    }
}