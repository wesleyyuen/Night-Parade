using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    public int coinOnHand  { get; private set; }

    void Start () {
        coinOnHand = FindObjectOfType<GameMaster> ().savedPlayerVariables.SavedPlayerCoinsOnHand;
    }
    
    public void pickUpCoin(int amt) {
        coinOnHand += amt;
    }
}