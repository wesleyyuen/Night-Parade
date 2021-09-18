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
            DontDestroyOnLoad(gameObject); // Contains all managers
        } else {
            Destroy(gameObject);
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
        SoundManager.Instance.PauseAll();
        PlayerData emptyData = new PlayerData();
        RequestSceneChange("Main_Menu", ref emptyData);
    }

    // can remove if play mode from _preload
    void SetUI(bool boolean)
    {
        DialogueManager.Instance.enabled = boolean;

        // Handle cursor
        Cursor.visible = !boolean;
        Cursor.lockState = !boolean ? CursorLockMode.Confined : CursorLockMode.Locked;

        if (boolean) {
            HealthUI.Instance.Intro();
            StaminaUI.Instance.Intro();
            MonUI.Instance.Intro();
        } else {
            HealthUI.Instance.Outro();
            StaminaUI.Instance.Outro();
            MonUI.Instance.Outro();
        }

        GameObject dialogueUI = GameObject.FindGameObjectWithTag ("DialogueUI");
        if (dialogueUI != null) dialogueUI.SetActive(boolean);
    }

    // Add when making a new area
    void FillAreaNameToIndexDictionary ()
    {
        areaNameToIndex.Add("Forest", 0);
    }
}