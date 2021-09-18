using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int SaveFileIndex { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public float CurrentStamina { get; private set; } // In Seconds
    public float MaxStamina { get; private set; } // In Seconds
    public int SavedPlayerCoinsOnHand { get; private set; }
    public int LastSavePoint { get; private set; }
    public float SavedPlayTime { get; private set; } // In Seconds
    public bool[] SavedInks { get; private set; }
    public int SavedOrbs { get; private set; }
    public Dictionary<string, int> SavedAreaPrgress { get; private set; }

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
        CurrentStamina = startingStamina;
        MaxStamina = startingStamina;
        SavedPlayerCoinsOnHand = 0;
        LastSavePoint = 0;
        SavedPlayTime = 0;
        SavedInks = new bool[GameMaster.numOfAreas];
        SavedOrbs = 0;
        SavedAreaPrgress = new Dictionary<string, int> ();
    }

    // For Saving and Loading (both inbetween and within session)
    public PlayerData(GameObject player, bool hardSave, int sceneIndex, int loadIndex)
    {
        SaveFileIndex = loadIndex;
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        PlayerAbilityController ability = player.GetComponent<PlayerAbilityController>();
        PlayerProgress progress = player.GetComponent<PlayerProgress>();
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (health != null) {
            CurrentHealth = health.currHealth;
            MaxHealth = health.maxHealth;
        }

        if (ability != null) {
            CurrentStamina = ability.currStamina;
            MaxStamina = ability.maxStamina;
        }

        if (progress != null) {
            SavedPlayTime = progress.GetPlayTimeInScene();
            SavedAreaPrgress = progress.areaProgress;
        }

        if (inventory != null) {
            SavedPlayerCoinsOnHand = inventory.coinOnHand;
            SavedInks = inventory.inks;
            SavedOrbs = inventory.orbs;
        }
        
        if (hardSave) {
            // change save point to current scene if manual saving i.e. saving at SavePoint
            LastSavePoint = sceneIndex;
        } else {
            if (ability != null)
                ability.StopUpdatingStamina();
        }
    }

    public bool IsValid()
    {
        return SaveFileIndex != 0;
    }
}