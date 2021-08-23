using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    static GameMaster _instance;
    public static GameMaster Instance {
        get  {return _instance; }
    }
    public static Dictionary<string, int> areaNameToIndex = new Dictionary<string, int> ();
    [SerializeField] string startingScene;
    public string prevScene {get; private set;}
    public string currentScene {get; private set;}
    public int startingHearts = 3;
    public float startingStamina = 3f;
    public const int numOfAreas = 5;
    [HideInInspector] public PlayerData savedPlayerData;
    const int healthPerHeart = 4;

    void Awake ()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
        
        // Initialize Dictionary
        FillAreaNameToIndexDictionary();
        UpdateCurrentScene();
        savedPlayerData = new PlayerData (startingHearts * healthPerHeart, startingStamina);

        // Load starting scene with saved player data
        RequestSceneChange(startingScene, ref savedPlayerData);
        SetUI(startingScene != "Main_Menu");
    }

    public void UpdateCurrentScene()
    {
        currentScene = SceneManager.GetActiveScene().name;
    }
    
    public void RequestSceneChange(string sceneToLoad, ref PlayerData currPlayerData)
    {
        if (sceneToLoad == "Main_Menu") {
            SetUI(false);
        } else if (currentScene == "Main_Menu" && sceneToLoad != "Main_Menu") {
            SetUI(true);
        }

        prevScene = currentScene;
        if (currPlayerData.IsValid())
            savedPlayerData = currPlayerData;
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    public void RequestSceneChangeToMainMenu()
    {
        PlayerData emptyData = new PlayerData();
        RequestSceneChange("Main_Menu", ref emptyData);
    }

    // can remove if play mode from _preload
    void SetUI(bool boolean)
    {
        DialogueManager.Instance.enabled = boolean;

        if (boolean) {
            FindObjectOfType<HealthUI>().Intro();
            FindObjectOfType<StaminaUI>().Intro();
        } else {
            FindObjectOfType<HealthUI>().Outro();
            FindObjectOfType<StaminaUI>().Outro();
        }

        GameObject monUI = GameObject.FindGameObjectWithTag ("MonUI");
        if (monUI != null) monUI.SetActive(boolean);
        GameObject dialogueUI = GameObject.FindGameObjectWithTag ("DialogueUI");
        if (dialogueUI != null) dialogueUI.SetActive(boolean);
    }

    // Add when making a new area
    void FillAreaNameToIndexDictionary ()
    {
        areaNameToIndex.Add("Forest", 0);
    }
}