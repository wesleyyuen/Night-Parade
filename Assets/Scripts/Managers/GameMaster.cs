using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster _instance;
    public static GameMaster Instance {
        get  {return _instance; }
    }
    private EventManager _eventManager;
    private InputManager _inputManager;
    [SerializeField] public Constant.SceneName startingScene = Constant.SceneName.Main_Menu;
    public string prevScene {get; private set;}
    public string currentScene {get; private set;}
    public static event System.Action Event_GameMasterInitalized;

    [Inject]
    public void Initialize(EventManager eventManager, InputManager inputManager)
    {
        _eventManager = eventManager;
        _inputManager = inputManager;
    }

    private void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject); // Contains all managers
            DontDestroyOnLoad(GameObject.FindGameObjectWithTag("UICanvas"));
        } else {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        Event_GameMasterInitalized?.Invoke();
        
        bool isTesting = startingScene != Constant.SceneName.Main_Menu && startingScene != Constant.SceneName._TestingChamber;
        SetUI(isTesting);
    }

    private void OnEnable()
    {
        _inputManager.Event_CheatInput_TestChamber += OnTestChamber;
    }

    private void OnDisable()
    {
        _inputManager.Event_CheatInput_TestChamber -= OnTestChamber;
    }

    private void OnTestChamber()
    {
        if (currentScene != "_TestingChamber")
            RequestSceneChange("_TestingChamber", ref SaveManager.Instance.savedPlayerData);
        else
            RequestSceneChange(prevScene, ref SaveManager.Instance.savedPlayerData);
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
            AudioManager.Instance.UnpauseAll();
            SetUI(true);
        }

        prevScene = currentScene;
        currentScene = sceneToLoad;

        // TODO: For some reason BOTH causes flickering 
        // yield return SceneManager.LoadSceneAsync(sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void RequestSceneChangeToMainMenu()
    {
        AudioManager.Instance.PauseAll();
        _eventManager.OnUIOutro();
        
        PlayerData emptyData = new PlayerData();
        RequestSceneChange(Constant.SceneName.Main_Menu.ToString(), ref emptyData);
    }

    // can remove if play mode from _preload
    public void SetUI(bool boolean)
    {
        // DialogueManager.Instance.enabled = boolean;

        // Handle cursor
        Cursor.visible = !boolean;
        Cursor.lockState = boolean ? CursorLockMode.Locked : CursorLockMode.Confined;

        if (boolean) {
            _eventManager.OnUIIntro();
        } else {
            _eventManager.OnUIOutro();
        }

        // GameObject dialogueUI = GameObject.FindGameObjectWithTag("DialogueUI");
        // if (dialogueUI != null)
        //     dialogueUI.SetActive(boolean);
    }
}