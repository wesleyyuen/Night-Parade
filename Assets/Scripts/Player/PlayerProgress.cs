using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour {
    [HideInInspector] public Dictionary<string, bool> areaProgress;

    void Awake () {
        areaProgress = FindObjectOfType<GameMaster> ().savedPlayerData.SavedAreaPrgress;
    }

    public float GetPlayTimeInScene () {
        return FindObjectOfType<GameMaster> ().savedPlayerData.SavedPlayTimeInSecs + Time.timeSinceLevelLoad;
    }
}