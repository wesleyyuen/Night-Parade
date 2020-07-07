using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData {

    public int SaveFileIndex { get; private set; }
    public int SavedPlayerHealth { get; private set; }
    public int SavedMaxPlayerHealth { get; private set; }
    public int SavedPlayerCoinsOnHand { get; private set; }
    public int LastSavePoint { get; private set; }
    public Dictionary<string, bool> savedAreaPrgress { get; private set; }

    // For Initializing
    public PlayerData (int startingHealth) {
        SaveFileIndex = 1; // New Game will override first save slot
        SavedPlayerHealth = startingHealth;
        SavedMaxPlayerHealth = startingHealth;
        SavedPlayerCoinsOnHand = 0;
        LastSavePoint = 0;
        savedAreaPrgress = new Dictionary<string, bool> ();
    }

    // For Saving and Loading (both inbetween and within session)
    public PlayerData (GameObject player, bool hardSave, int sceneIndex, int loadIndex) {
        SaveFileIndex = loadIndex;
        SavedPlayerHealth = player.GetComponent<PlayerHealth> ().currHealth;
        SavedMaxPlayerHealth = player.GetComponent<PlayerHealth> ().maxNumOfHeart;
        SavedPlayerCoinsOnHand = player.GetComponent<PlayerInventory> ().coinOnHand;
        savedAreaPrgress = player.GetComponent<PlayerProgress> ().areaProgress;
        if (hardSave) {
            Debug.Log ("Changing savepoint from " + SceneManager.GetSceneByBuildIndex (LastSavePoint).name + " to " + SceneManager.GetSceneByBuildIndex (sceneIndex).name);
            LastSavePoint = sceneIndex;
        }
    }

    /*
        // For when player Died, save progress (with percentage of coins) and restart at last save point
        public PlayerData (GameObject player, float percentOfCoinsLostAfterDeath, int loadIndex) {
            SaveFileIndex = loadIndex;
            SavedPlayerHealth = 1;
            SavedMaxPlayerHealth = player.GetComponent<PlayerHealth> ().maxNumOfHeart;
            SavedPlayerCoinsOnHand = Mathf.RoundToInt (player.GetComponent<PlayerInventory> ().coinOnHand * (1 - percentOfCoinsLostAfterDeath));
            //LastSavePoint = LastSavePoint;
        }
        */
}