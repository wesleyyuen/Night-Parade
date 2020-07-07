using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour {
    [HideInInspector] public Dictionary<string, bool> areaProgress;


    // TODO: fix ordering issue
    void Awake () {
        areaProgress = FindObjectOfType<GameMaster> ().savedPlayerData.savedAreaPrgress;
    }
}