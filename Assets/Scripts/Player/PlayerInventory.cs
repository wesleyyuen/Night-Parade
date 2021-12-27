using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int monOnHand;
    public int MonOnHand { get { return monOnHand; } }
    private int recentMonDelta;
    public int RecentMonDelta { get { return recentMonDelta; } }
    public bool[] inks { get; private set; }
    public int orbs { get; private set; }
    public event System.Action Event_MonChange;

    private void Start()
    {
        // Get saved data from SaveManager
        monOnHand = SaveManager.Instance.savedPlayerData.CoinsOnHand;
        recentMonDelta = 0;
        inks = SaveManager.Instance.savedPlayerData.SavedInks;
        orbs = SaveManager.Instance.savedPlayerData.SavedOrbs;
    }

    public void PickUpCoin(int amt)
    {
        // TODO: mon deltas listen for 3 seconds before adding to monOnHand  
        recentMonDelta = amt;
        monOnHand += amt;

        // Update Mon UI
        Event_MonChange?.Invoke();
    }

    public void PickUpInk(int areaIndex)
    {
        inks[areaIndex] = true;
    }

    public void PickUpOrb()
    {
        orbs++;
    }
}