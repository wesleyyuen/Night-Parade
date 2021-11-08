using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Forest_Area4 : MonoBehaviour {
    [SerializeField] private Transform playerGroup;
    [SerializeField] private Transform area2SpawnPoint;
    [SerializeField] private PlayableDirector area2SpawnAnimation;
    [SerializeField] private Transform area3SpawnPoint;
    [SerializeField] private PlayableDirector area3SpawnAnimation;
    [SerializeField] private Transform area7SpawnPoint;
    void Start () {
        

        if (GameMaster.Instance.prevScene == "Forest_Area2") {
            foreach (Transform child in playerGroup) {
                child.position = area2SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (-1f, 1f, 1f);
            }
            area2SpawnAnimation.Play ();
        } else if (GameMaster.Instance.prevScene == "Forest_Area3") {
            foreach (Transform child in playerGroup) {
                child.position = area3SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }            
            area3SpawnAnimation.Play ();
        } else if (GameMaster.Instance.prevScene == "Forest_Area7") {
            foreach (Transform child in playerGroup) {
                child.position = area7SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }                      
        }
    }
}