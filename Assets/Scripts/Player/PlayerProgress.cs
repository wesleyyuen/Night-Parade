using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    GameMaster _gameMaster;
    [HideInInspector] public Dictionary<string, bool> areaProgress;

    void Awake ()
    {
        _gameMaster = GameMaster.Instance;
        if (_gameMaster) {
            areaProgress = _gameMaster.savedPlayerData.SavedAreaPrgress;
        }
    }
    // void Update ()
    // {
    //     if (!_gameMaster) {
    //         _gameMaster = GameMaster.Instance;
    //         if (_gameMaster)
    //             areaProgress = _gameMaster.savedPlayerData.SavedAreaPrgress;
    //     }
    // }

    public float GetPlayTimeInScene ()
    {
        return GameMaster.Instance.savedPlayerData.SavedPlayTime + Time.timeSinceLevelLoad;
    }
}