using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    public int coinOnHand  { get; private set; }

    void Start () {
        coinOnHand = FindObjectOfType<GameMaster> ().savedPlayerCoinsOnHand;
    }
    
    public void pickUpCoin(int amt) {
        coinOnHand += amt;
    }
}