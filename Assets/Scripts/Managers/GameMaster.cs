using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster _instance;
    public static GameMaster Instance {
        get  {return _instance; }
    }
    [SerializeField] public Constant.SceneName startingScene;
    [SerializeField] bool _enabledTestChamber;
    public string prevScene {get; private set;}
    public string currentScene {get; private set;}
    public event System.Action Event_GameMasterInitalized;
    
    // Static UI Events to avoid OnEnable/OnDisable Null References
    public static event System.Action Event_UIIntro;
    public static event System.Action Event_UIOutro;
    private bool _isInTestingChamber;

    private void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Contains all managers
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("UICanvas"));
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
    
    private void Start()
    {
        Event_GameMasterInitalized?.Invoke();
    }

    private void OnTestingChamber(InputAction.CallbackContext context)
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
        Event_UIOutro?.Invoke();
        
        PlayerData emptyData = new PlayerData();
        RequestSceneChange(Constant.SceneName.Main_Menu.ToString(), ref emptyData);
    }

    // can remove if play mode from _preload
    private void SetUI(bool boolean)
    {
        DialogueManager.Instance.enabled = boolean;

        // Handle cursor
        Cursor.visible = !boolean;
        Cursor.lockState = boolean ? CursorLockMode.Locked : CursorLockMode.Confined;

        if (boolean) {
            Event_UIIntro?.Invoke();
        } else {
            Event_UIOutro?.Invoke();
        }

        GameObject dialogueUI = GameObject.FindGameObjectWithTag("DialogueUI");
        if (dialogueUI != null)
            dialogueUI.SetActive(boolean);
    }
}