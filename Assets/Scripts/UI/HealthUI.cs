using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class HealthUI : MonoBehaviour {
    private static HealthUI Instance;
    private PlayerHealth playerHealth;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite threeQuartersHeart;
    [SerializeField] private Sprite halfHeart;
    [SerializeField] private Sprite quarterHeart;
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

    public void UpdateHearts() {
        playerHealth = FindObjectOfType<PlayerHealth> ();
        if (playerHealth == null) return;

        int numOfFullHearts = playerHealth.currHealth / 4;
        int maxHearts = playerHealth.maxHealth / 4;

        // Display hearts
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = (i < numOfFullHearts) ? fullHeart : emptyHeart;

            // disable hearts that exceed current maximum health
            hearts[i].enabled = i < maxHearts;
        }

        // Handle Reminder
        int reminder = playerHealth.currHealth % 4;
        if (reminder == 0) return;
        hearts[numOfFullHearts].sprite = (reminder == 3) ? threeQuartersHeart
                                                   : (reminder == 2) ? halfHeart : quarterHeart;
    }
}