using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    private static GameMaster Instance;
    public string startingScene;
    public static string prevScene = "";
    public static string currentScene = "";
    public int startingHealth;
    public PlayerData savedPlayerData;
    public bool[][] savedScenesInteractions; 

    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
        UpdateCurrentScene ();
        savedPlayerData = new PlayerData (startingHealth, 0);
        if (startingScene == "Main_Menu") SetUIHelper (false); // can remove if play mode from _preload
        RequestSceneChange (startingScene, savedPlayerData);
    }

    public void UpdateCurrentScene () {
        currentScene = SceneManager.GetActiveScene ().name;
    }

    public string GetPrevScene () {
        return prevScene;
    }

    public void RequestSceneChange (string sceneToLoad, PlayerData currPlayerData) {
        prevScene = currentScene;
        savedPlayerData = currPlayerData;
        SceneManager.LoadScene (sceneToLoad);
    }

    // can remove if play mode from _preload
    void SetUIHelper (bool boolean) {
        FindObjectOfType<CameraPeeking> ().enabled = boolean;
        FindObjectOfType<DialogueManager> ().enabled = boolean;
        GameObject.FindGameObjectWithTag ("MonUI").transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
        GameObject.FindGameObjectWithTag ("HealthUI").transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
        GameObject.FindGameObjectWithTag ("DialogueUI").transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
    }
}