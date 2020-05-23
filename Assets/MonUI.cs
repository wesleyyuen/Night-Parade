using UnityEngine;
using UnityEngine.UI;

public class MonUI : MonoBehaviour {
    
    public Text monText;
    PlayerInventory playerInventory;

    void Start () {
        playerInventory = FindObjectOfType<PlayerInventory> ();
    }

    void Update () {
        if (playerInventory == null) playerInventory = FindObjectOfType<PlayerInventory> ();
        if (playerInventory == null) return;

        monText.text = playerInventory.coinOnHand.ToString();
    }
}