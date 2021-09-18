using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    GameMaster _gameMaster;
    [HideInInspector] public Dictionary<string, int> areaProgress {get; private set;}

    void Awake ()
    {
        _gameMaster = GameMaster.Instance;
        if (_gameMaster) {
            areaProgress = _gameMaster.savedPlayerData.SavedAreaPrgress;
        }
    }

    public float GetPlayTimeInScene ()
    {
        return GameMaster.Instance.savedPlayerData.SavedPlayTime + Time.timeSinceLevelLoad;
    }

    public bool HasPlayerProgress(string key)
    {
        return areaProgress.ContainsKey(key);
    }

    public int GetPlayerProgress(string key)
    {
        if (areaProgress.ContainsKey(key))
            return areaProgress[key];
        else {
            Debug.Log("Player progress does not contain key '" + key + "'!");
            return -1;
        }
    }

    public void AddPlayerProgress(string key, int value, bool shouldOverride = true)
    {
        if (areaProgress.ContainsKey(key))  {
            if (shouldOverride)
                areaProgress[key] = value;
        } else
            areaProgress.Add(key, value);
    }
}