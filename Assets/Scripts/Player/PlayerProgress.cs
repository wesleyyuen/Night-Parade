using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour {
    private GameMaster gameMaster;
    [HideInInspector] public Dictionary<string, bool> areaProgress;

    private void Awake () {
        gameMaster = FindObjectOfType<GameMaster> ();
        if (gameMaster) {
            areaProgress = gameMaster.savedPlayerData.SavedAreaPrgress;
        }
    }

    private void Update () {
        if (!gameMaster) {
            gameMaster = FindObjectOfType<GameMaster> ();
            if (gameMaster) {
            areaProgress = gameMaster.savedPlayerData.SavedAreaPrgress;
        }
        }
    }

    public float GetPlayTimeInScene () {
        return FindObjectOfType<GameMaster> ().savedPlayerData.SavedPlayTimeInSecs + Time.timeSinceLevelLoad;
    }
}