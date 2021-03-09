using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hatsumura : MonoBehaviour {
    [SerializeField] private Transform playerGroup;
    [SerializeField] private Transform area3SpawnPoint;
    [SerializeField] private TextMeshProUGUI hatsumuraText;
    [SerializeField] private float textShowingTime;
    [SerializeField] private float textFadingTime;
    void Start () {
        GameMaster.Instance.UpdateCurrentScene ();
        StartCoroutine (Common.FadeText(hatsumuraText, textShowingTime, textFadingTime));

        if (GameMaster.Instance.GetPrevScene () == "Forest_Area3") {
            foreach (Transform child in playerGroup) {
                child.position = area3SpawnPoint.position;
                if (child.name == "Player")
                    child.localScale = new Vector3 (1f, 1f, 1f);
            }
        }
    }

    
}