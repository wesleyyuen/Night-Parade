using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {

    public int SaveFileIndex { get; private set; }
    public int SavedPlayerHealth { get; private set; }
    public int SavedMaxPlayerHealth { get; private set; }
    public int SavedPlayerCoinsOnHand { get; private set; }
    public int LastSavePoint { get; private set; }
    public float SavedPlayTimeInSecs { get; private set; }
    public bool[] SavedInks { get; private set; }
    public int SavedOrbs { get; private set; }
    public Dictionary<string, bool> SavedAreaPrgress { get; private set; }

    // For Initializing
    public PlayerData (int startingHealth) {
        SaveFileIndex = 1; // New Game will override first save slot
        SavedPlayerHealth = startingHealth;
        SavedMaxPlayerHealth = startingHealth;
        SavedPlayerCoinsOnHand = 0;
        LastSavePoint = 0;
        SavedPlayTimeInSecs = 0;
        SavedInks = new bool[GameMaster.numOfAreas];
        SavedOrbs = 0;
        SavedAreaPrgress = new Dictionary<string, bool> ();
    }

    // For Saving and Loading (both inbetween and within session)
    public PlayerData (GameObject player, bool hardSave, int sceneIndex, int loadIndex) {
        SaveFileIndex = loadIndex;
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth> ();
        PlayerProgress playerProgress = player.GetComponent<PlayerProgress> ();
        SavedPlayerHealth = player.GetComponent<PlayerHealth> ().currHealth;
        SavedMaxPlayerHealth = player.GetComponent<PlayerHealth> ().maxNumOfHeart;
        SavedPlayerCoinsOnHand = player.GetComponent<PlayerInventory> ().coinOnHand;
        SavedPlayTimeInSecs = player.GetComponent<PlayerProgress> ().GetPlayTimeInScene ();
        SavedInks = player.GetComponent<PlayerInventory>().inks;
        SavedOrbs = player.GetComponent<PlayerInventory>().orbs;
        SavedAreaPrgress = player.GetComponent<PlayerProgress> ().areaProgress;
        if (hardSave) {
            // change save point to current scene if manual saving i.e. saving at SavePoint
            LastSavePoint = sceneIndex;
        } // else do not change last save point
    }
}