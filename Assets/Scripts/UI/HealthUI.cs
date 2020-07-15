using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class HealthUI : MonoBehaviour {
    private static HealthUI Instance;
    private PlayerHealth playerHealth;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    
    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (Instance);
        } else {
            Destroy (gameObject);
        }
    }

    void Start() {
        playerHealth = FindObjectOfType<PlayerHealth> ();
    }

    void Update () {
        // repopulate variables after loading new scene
        if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth> ();
        if (playerHealth == null) return;
        
        // Display hearts
        for (int i = 0; i < hearts.Length; i++) {
            // make lost health empty hearts
            hearts[i].sprite = (i < playerHealth.currHealth) ? fullHeart : emptyHeart;
            // disable hearts that exceed current maximum health
            hearts[i].enabled = i < playerHealth.maxNumOfHeart;
        }
    }
}