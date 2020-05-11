using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    private static GameMaster Instance;
    public string startingScene;
    public static string prevScene = "";
    public static string currentScene = "";
    public int startingHealth;
    public int savedPlayerHealth { get; private set; }

    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
        UpdateCurrentScene ();
        requestSceneChange (startingScene, startingHealth);
    }

    public void UpdateCurrentScene () {
        currentScene = SceneManager.GetActiveScene ().name;
    }

    public string getPrevScene () {
        return prevScene;
    }

    public void requestSceneChange (string sceneToLoad, int currHealth) {
        prevScene = currentScene;
        savedPlayerHealth = currHealth;
        SceneManager.LoadScene (sceneToLoad);
    }
}