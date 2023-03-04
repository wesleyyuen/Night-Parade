using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInventory : MonoBehaviour
{
    private int monOnHand;
    public int MonOnHand { get { return monOnHand; } }
    private int recentMonDelta;
    public int RecentMonDelta { get { return recentMonDelta; } }
    private EventManager _eventManager;

    [Inject]
    public void Initialize(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    private void Start()
    {
        // Get saved data from SaveManager
        monOnHand = SaveManager.Instance.savedPlayerData.CoinsOnHand;
        recentMonDelta = 0;
    }

    public void PickUpCoin(int amt)
    {
        int prev = monOnHand;

        // TODO: mon deltas listen for 3 seconds before adding to monOnHand  
        recentMonDelta = amt;
        monOnHand += amt;

        // Update Mon UI
        _eventManager.OnPlayerMonIncreased(prev, monOnHand);
    }
}