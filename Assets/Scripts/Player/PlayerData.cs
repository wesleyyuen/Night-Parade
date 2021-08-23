using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int SaveFileIndex { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public float MaxStamina { get; private set; } // In Seconds
    public int SavedPlayerCoinsOnHand { get; private set; }
    public int LastSavePoint { get; private set; }
    public float SavedPlayTime { get; private set; } // In Seconds
    public bool[] SavedInks { get; private set; }
    public int SavedOrbs { get; private set; }
    public Dictionary<string, bool> SavedAreaPrgress { get; private set; }

    // Empty
    public PlayerData()
    {
        SaveFileIndex = 0;
    }

    // For Initializing
    public PlayerData (int startingHearts, float startingStamina)
    {
        SaveFileIndex = 1; // New Game will override first save slot
        CurrentHealth = startingHearts;
        MaxHealth = startingHearts;
        MaxStamina = startingStamina;
        SavedPlayerCoinsOnHand = 0;
        LastSavePoint = 0;
        SavedPlayTime = 0;
        SavedInks = new bool[GameMaster.numOfAreas];
        SavedOrbs = 0;
        SavedAreaPrgress = new Dictionary<string, bool> ();
    }

    // For Saving and Loading (both inbetween and within session)
    public PlayerData (GameObject player, bool hardSave, int sceneIndex, int loadIndex)
    {
        SaveFileIndex = loadIndex;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth> ();
        PlayerProgress playerProgress = player.GetComponent<PlayerProgress> ();
        CurrentHealth = player.GetComponent<PlayerHealth>().currHealth;
        MaxHealth = player.GetComponent<PlayerHealth>().maxHealth;
        MaxStamina = player.GetComponent<PlayerAbilityController>().maxStamina;
        SavedPlayerCoinsOnHand = player.GetComponent<PlayerInventory>().coinOnHand;
        SavedPlayTime = player.GetComponent<PlayerProgress>().GetPlayTimeInScene ();
        SavedInks = player.GetComponent<PlayerInventory>().inks;
        SavedOrbs = player.GetComponent<PlayerInventory>().orbs;
        SavedAreaPrgress = player.GetComponent<PlayerProgress> ().areaProgress;
        if (hardSave) {
            // change save point to current scene if manual saving i.e. saving at SavePoint
            LastSavePoint = sceneIndex;
        } // else do not change last save point
    }

    public bool IsValid()
    {
        return SaveFileIndex != 0;
    }
}