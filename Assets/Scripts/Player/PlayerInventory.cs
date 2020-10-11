using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    public int coinOnHand { get; private set; }
    [HideInInspector] public bool[] inks;
    [HideInInspector] public int orbs;

    void Start () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        // Get saved data from Gamemaster
        coinOnHand = gameMaster.savedPlayerData.SavedPlayerCoinsOnHand;
        inks = gameMaster.savedPlayerData.SavedInks;
        orbs = gameMaster.savedPlayerData.SavedOrbs;

        FindObjectOfType<MonUI>().UpdateMon();
    }

    public void PickUpCoin (int amt) {
        coinOnHand += amt;

        // Change Mon UI
        FindObjectOfType<MonUI>().UpdateMon();
    }

    public void PickUpInk (int areaIndex) {
        inks[areaIndex] = true;
    }

    public void PickUpOrb () {
        orbs++;
    }
}