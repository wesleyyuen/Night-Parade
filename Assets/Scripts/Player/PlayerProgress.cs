using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour {
    private GameMaster gameMaster;
    [HideInInspector] public Dictionary<string, bool> areaProgress;

    private void Awake () {
        gameMaster = GameMaster.Instance;
        if (gameMaster) {
            areaProgress = gameMaster.savedPlayerData.SavedAreaPrgress;
        }
    }
    private void Update () {
        if (!gameMaster) {
            gameMaster = GameMaster.Instance;
            if (gameMaster) {
            areaProgress = gameMaster.savedPlayerData.SavedAreaPrgress;
        }
        }
    }

    public float GetPlayTimeInScene () {
        return GameMaster.Instance.savedPlayerData.SavedPlayTimeInSecs + Time.timeSinceLevelLoad;
    }
}