using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    private static GameMaster Instance;
    public string startingScene;
    public static string prevScene = "";
    public static string currentScene = "";
    public int startingHealth;
    public int savedPlayerHealth { get; private set; }
    public int savedPlayerCoinsOnHand { get; private set; }

    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
        UpdateCurrentScene ();
        requestSceneChange (startingScene, startingHealth, 0);
    }

    public void UpdateCurrentScene () {
        currentScene = SceneManager.GetActiveScene ().name;
    }

    public string getPrevScene () {
        return prevScene;
    }

    public void requestSceneChange (string sceneToLoad, int currHealth, int coinsOnHand) {
        prevScene = currentScene;
        UpdatePlayerVariables(currHealth, coinsOnHand);
        SceneManager.LoadScene (sceneToLoad);
    }

    void UpdatePlayerVariables(int currHealth, int coinsOnHand) {
        savedPlayerHealth = currHealth;
        savedPlayerCoinsOnHand = coinsOnHand;
    }
}