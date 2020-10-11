using TMPro;
using UnityEngine;

public class MonUI : MonoBehaviour {

    private static MonUI Instance;
    private PlayerInventory playerInventory;
    [SerializeField] private TextMeshProUGUI monText;

    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
    }

    void Start () {
        playerInventory = FindObjectOfType<PlayerInventory> ();
    }

    public void UpdateMon() {
        playerInventory = FindObjectOfType<PlayerInventory> ();
        if (playerInventory == null) return;

        // Display current coins on hand as text
        monText.text = playerInventory.coinOnHand.ToString ();
    }
}