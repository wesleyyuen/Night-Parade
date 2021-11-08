using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int coinOnHand { get; private set; }
    public bool[] inks { get; private set; }
    public int orbs { get; private set; }

    void Start()
    {
        // Get saved data from SaveManager
        coinOnHand = SaveManager.Instance.savedPlayerData.CoinsOnHand;
        inks = SaveManager.Instance.savedPlayerData.SavedInks;
        orbs = SaveManager.Instance.savedPlayerData.SavedOrbs;
    }

    public void PickUpCoin (int amt)
    {
        coinOnHand += amt;

        // Change Mon UI
        MonUI.Instance.ShowMonChange();
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