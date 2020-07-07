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
        if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth> ();
        if (playerHealth == null) return;
        
        for (int i = 0; i < hearts.Length; i++) {
            hearts[i].sprite = (i < playerHealth.currHealth) ? fullHeart : emptyHeart;
            hearts[i].enabled = i < playerHealth.maxNumOfHeart;
        }
    }
}