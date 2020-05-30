using UnityEngine;

public class PlayerVariables {
    public int SavedPlayerHealth { get; private set; }
    public int SavedPlayerCoinsOnHand { get; private set; }
    public PlayerVariables (int playerHealth, int playerCoinsOnHand) {
        SavedPlayerHealth = playerHealth;
        SavedPlayerCoinsOnHand = playerCoinsOnHand;
    }
}
