using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int coinOnHand { get; private set; }
    public bool[] inks { get; private set; }
    public int orbs { get; private set; }

    void Start ()
    {
        // Get saved data from Gamemaster
        coinOnHand = GameMaster.Instance.savedPlayerData.SavedPlayerCoinsOnHand;
        inks = GameMaster.Instance.savedPlayerData.SavedInks;
        orbs = GameMaster.Instance.savedPlayerData.SavedOrbs;
    }

    public void PickUpCoin (int amt)
    {
        coinOnHand += amt;

        // Change Mon UI
        MonUI.Instance.ShowMonChange();
        // FindObjectOfType<MonUI>().ShowMonChange();
    }

    public void PickUpInk (int areaIndex)
    {
        inks[areaIndex] = true;
    }

    public void PickUpOrb ()
    {
        orbs++;
    }
}