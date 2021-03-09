using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    private static GameMaster instance;
    public static GameMaster Instance {
        get  {return instance; }
    }
    public static Dictionary<string, int> areaNameToIndex = new Dictionary<string, int> ();
    [SerializeField] private string startingScene;
    private string prevScene = "";
    private string currentScene = "";
    public int startingHearts = 3;
    public const int numOfAreas = 5;
    [HideInInspector] public PlayerData savedPlayerData;
    private const int healthPerHeart = 4;

    void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
        
        // Initialize Dictionary
        FillAreaNameToIndexDictionary ();
        UpdateCurrentScene ();
        savedPlayerData = new PlayerData (startingHearts * healthPerHeart);
        if (startingScene == "Main_Menu") SetUIHelper (false); // can remove if play mode from _preload

        // Load starting scene with saved player data
        RequestSceneChange (startingScene, ref savedPlayerData);
    }

    public void UpdateCurrentScene () {
        currentScene = SceneManager.GetActiveScene ().name;
    }

    public string GetPrevScene () {
        return prevScene;
    }

    public void RequestSceneChange (string sceneToLoad, ref PlayerData currPlayerData) {
        prevScene = currentScene;
        savedPlayerData = currPlayerData;
        SceneManager.LoadScene (sceneToLoad);
    }

    // overload for buildIndex instead of scene Name
    public void RequestSceneChange (int sceneToLoad, ref PlayerData currPlayerData) {
        prevScene = currentScene;
        savedPlayerData = currPlayerData;
        SceneManager.LoadScene (sceneToLoad);
    }

    // can remove if play mode from _preload
    void SetUIHelper (bool boolean) {
        DialogueManager.Instance.enabled = boolean;
        GameObject.FindGameObjectWithTag ("MonUI").transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
        GameObject.FindGameObjectWithTag ("HealthUI").transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
        GameObject.FindGameObjectWithTag ("DialogueUI").transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
    }

    // Add when making a new area
    private void FillAreaNameToIndexDictionary () {
        areaNameToIndex.Add ("Forest", 0);
    }
}