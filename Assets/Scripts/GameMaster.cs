using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    private static GameMaster Instance;
    public string startingScene;
    public static string prevScene = "";
    public static string currentScene = "";
    public int startingHealth;
    public PlayerVariables savedPlayerVariables { get; private set; }

    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
        UpdateCurrentScene ();
        savedPlayerVariables = new PlayerVariables (startingHealth, 0);
        requestSceneChange (startingScene, savedPlayerVariables);
    }
    public void UpdateCurrentScene () {
        currentScene = SceneManager.GetActiveScene ().name;
    }

    public string getPrevScene () {
        return prevScene;
    }

    public void requestSceneChange (string sceneToLoad, PlayerVariables currPlayerVariables) {
        prevScene = currentScene;
        savedPlayerVariables = currPlayerVariables;
        SceneManager.LoadScene (sceneToLoad);
    }
}