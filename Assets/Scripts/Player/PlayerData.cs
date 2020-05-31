using UnityEngine;

[System.Serializable]
public class PlayerData {

    public int SaveFileIndex { get; private set; }
    public int SavedPlayerHealth { get; private set; }
    public int SavedMaxPlayerHealth { get; private set; }
    public int SavedPlayerCoinsOnHand { get; private set; }
    public int LastSavePoint { get; private set; }

    // For Initializing
    public PlayerData (int playerHealth, int playerCoinsOnHand) {
        SaveFileIndex = 1;  // New Game will override first save slot
        SavedPlayerHealth = playerHealth;
        SavedMaxPlayerHealth = playerHealth;
        SavedPlayerCoinsOnHand = playerCoinsOnHand;
        LastSavePoint = LastSavePoint;
    }

    // For Saving and Loading (both inbetween and within session)
    public PlayerData (GameObject player, int sceneIndex, int loadIndex) {
        SaveFileIndex = loadIndex;
        SavedPlayerHealth = player.GetComponent<PlayerHealth> ().currHealth;
        SavedMaxPlayerHealth = player.GetComponent<PlayerHealth> ().maxNumOfHeart;
        SavedPlayerCoinsOnHand = player.GetComponent<PlayerInventory>().coinOnHand;
        if (sceneIndex == 0) LastSavePoint = LastSavePoint;
        else LastSavePoint = sceneIndex;
    }
}
