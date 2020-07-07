using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    public int coinOnHand { get; private set; }
    [HideInInspector] public bool[] inks;
    [HideInInspector] public int orbs;

    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        coinOnHand = gameMaster.savedPlayerData.SavedPlayerCoinsOnHand;
        inks = new bool[gameMaster.numOfAreas];
        orbs = 0;
    }

    public void PickUpCoin (int amt) {
        coinOnHand += amt;
    }

    public void PickUpInk (int areaIndex) {
        inks[areaIndex] = true;
        Debug.Log (inks);
    }

    public void PickUpOrb () {
        orbs++;
    }
}