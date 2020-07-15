using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hatsumura : MonoBehaviour {
    [SerializeField] private Transform player;
    [SerializeField] private Transform area3SpawnPoint;
    [SerializeField] private GameObject hatsumuraText;
    [SerializeField] private float textShowingTime;
    [SerializeField] private float textFadingTime;
    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        gameMaster.UpdateCurrentScene ();
        StartCoroutine (FindObjectOfType<Common>().FadeText(hatsumuraText, textShowingTime, textFadingTime));

        if (gameMaster.GetPrevScene () == "Forest_Area3") {
            player.position = area3SpawnPoint.position;
            player.localScale = new Vector3 (1f, 1f, 1f);
        }
    }

    
}