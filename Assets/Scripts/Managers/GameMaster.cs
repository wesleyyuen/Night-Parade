using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    static GameMaster _instance;
    public static GameMaster Instance {
        get  {return _instance; }
    }
    [SerializeField] public Constant.SceneName startingScene;
    [SerializeField] bool _enabledTestChamber;
    public string prevScene {get; private set;}
    public string currentScene {get; private set;}
    bool _isInTestingChamber;

    void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Contains all managers
        } else {
            Destroy(gameObject);
        }

        bool isTesting = startingScene != Constant.SceneName.Main_Menu;
        SetUI(isTesting);

        // Handle Input
        if (_enabledTestChamber) {
            InputMaster input = new InputMaster();
            input.Testing.TestingChamber.Enable();
            input.Testing.TestingChamber.started += OnTestingChamber;
        }
    }

    void OnTestingChamber(InputAction.CallbackContext context)
    {
        if (!_isInTestingChamber)
            RequestSceneChange("_TestingChamber", ref SaveManager.Instance.savedPlayerData);
        else
            RequestSceneChange(prevScene, ref SaveManager.Instance.savedPlayerData);

        _isInTestingChamber = !_isInTestingChamber;
    }

    public void RequestSceneChange(string sceneToLoad, ref PlayerData currPlayerData)
    {
        string mainMenu = Constant.SceneName.Main_Menu.ToString();

        if (currPlayerData.IsValid()) {
            SaveManager.Instance.savedPlayerData = currPlayerData;
            SaveManager.Instance.savedSceneData = currPlayerData.SceneData;
        }

        if (sceneToLoad == mainMenu) {
            SetUI(false);
        } else if (currentScene == mainMenu && sceneToLoad != mainMenu) {
            SetUI(true);
        }

        prevScene = currentScene;
        currentScene = sceneToLoad;

        // SceneManager.LoadSceneAsync(sceneToLoad);
        // TODO: For some reason LoadSceneAsync causes flickering again
        SceneManager.LoadScene(sceneToLoad);
    }

    public void RequestSceneChangeToMainMenu()
    {
        SoundManager.Instance.PauseAll();
        MonUI.Instance.Outro();
        HealthUI.Instance.Outro();
        
        PlayerData emptyData = new PlayerData();
        RequestSceneChange(Constant.SceneName.Main_Menu.ToString(), ref emptyData);
    }

    // can remove if play mode from _preload
    void SetUI(bool boolean)
    {
        DialogueManager.Instance.enabled = boolean;

        // Handle cursor
        Cursor.visible = !boolean;
        Cursor.lockState = boolean ? CursorLockMode.Locked : CursorLockMode.Confined;

        if (boolean) {
            HealthUI.Instance.Intro();
            StaminaUI.Instance.Intro();
            MonUI.Instance.Intro();
        } else {
            HealthUI.Instance.Outro();
            StaminaUI.Instance.Outro();
            MonUI.Instance.Outro();
        }

        GameObject dialogueUI = GameObject.FindGameObjectWithTag("DialogueUI");
        if (dialogueUI != null)
            dialogueUI.SetActive(boolean);
    }
}