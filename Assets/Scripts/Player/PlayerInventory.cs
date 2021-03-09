using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {

    public int coinOnHand { get; private set; }
    [HideInInspector] public bool[] inks;
    [HideInInspector] public int orbs;

    void Start () {
        // Get saved data from Gamemaster
        coinOnHand = GameMaster.Instance.savedPlayerData.SavedPlayerCoinsOnHand;
        inks = GameMaster.Instance.savedPlayerData.SavedInks;
        orbs = GameMaster.Instance.savedPlayerData.SavedOrbs;

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