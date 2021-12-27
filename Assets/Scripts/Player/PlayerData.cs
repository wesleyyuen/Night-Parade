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
    public int CoinsOnHand { get; private set; }
    public int LastSavePoint { get; private set; }
    public float PlayTime { get; private set; } // In Seconds
    public bool[] SavedInks { get; private set; }
    public int SavedOrbs { get; private set; }
    public Dictionary<string, SceneData> SceneData { get; private set; }

    // Empty
    public PlayerData()
    {
        SaveFileIndex = 0;
    }

    // For New Game
    // public PlayerData (int startingHearts, float startingStamina)
    // {
    //     SaveFileIndex = 1; // New Game will override first save slot
    //     CurrentHealth = startingHearts;
    //     MaxHealth = startingHearts;
    //     CurrentStamina = startingStamina;
    //     MaxStamina = startingStamina;
    //     CoinsOnHand = 0;
    //     LastSavePoint = 0;
    //     PlayTime = 0;
    //     SavedInks = new bool[GameMaster.numOfAreas];
    //     SavedOrbs = 0;
    //     AreaPrgress = new Dictionary<string, int> ();
    // }

    // For Loading inbetween sessions (Continue Game)
    public PlayerData(
        int saveFileIndex,
        int maxHealth,
        float maxStamina,
        int coinsOnHand,
        int lastSavePoint,
        float playTime,
        bool[] savedInks,
        int savedOrbs,
        Dictionary<string, SceneData> sceneData
    )
    {
        SaveFileIndex = saveFileIndex;
        CurrentHealth = maxHealth;
        MaxHealth = maxHealth;
        CurrentStamina = maxStamina;
        MaxStamina = maxStamina;
        CoinsOnHand = coinsOnHand;
        LastSavePoint = lastSavePoint;
        PlayTime = playTime;
        SavedInks = savedInks;
        SavedOrbs = savedOrbs;
        SceneData = sceneData;
    }

    // For Saving (both inbetween and within session) and Loading within session
    public PlayerData(GameObject player, bool hardSave, int sceneIndex, int loadIndex)
    {
        SaveFileIndex = loadIndex;
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        PlayerAbilityController ability = player.GetComponent<PlayerAbilityController>();
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (health != null) {
            CurrentHealth = health.currHealth;
            MaxHealth = health.maxHealth;
        }

        if (ability != null) {
            CurrentStamina = ability.currStamina;
            MaxStamina = ability.maxStamina;
        }

        PlayTime = SaveManager.Instance.GetPlayTimeInScene();
        SceneData = SaveManager.Instance.savedSceneData;

        if (inventory != null) {
            CoinsOnHand = inventory.MonOnHand;
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