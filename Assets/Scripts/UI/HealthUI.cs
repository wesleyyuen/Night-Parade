using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class HealthUI : MonoBehaviour {
    private static HealthUI Instance;
    PlayerHealth playerHealth;
    public int numOfHearts;
    public float heartLocalScale = 0.2f;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (Instance);
        } else {
            Destroy (gameObject);
        }
        playerHealth = FindObjectOfType<PlayerHealth> ();
    }

    // Update is called once per frame
    void Update () {
        if (playerHealth == null) playerHealth = FindObjectOfType<PlayerHealth> ();
        if (playerHealth == null) return;
        
        for (int i = 0; i < hearts.Length; i++) {
            if (i < playerHealth.currHealth) {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }
}