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

    void Update () {
        if (playerInventory == null) playerInventory = FindObjectOfType<PlayerInventory> ();
        if (playerInventory == null) return;

        monText.text = playerInventory.coinOnHand.ToString ();
    }
}