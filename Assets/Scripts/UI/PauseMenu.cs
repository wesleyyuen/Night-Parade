using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isPuased = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject optionsMenuUI;
    InputMaster _input;

    void Awake()
    {
        _input = new InputMaster();

        _input.UI.Pause.performed += OnPauseOrResume;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void OnEnable()
    {
        _input.UI.Pause.Enable();
    }

    void OnDisable()
    {
        _input.UI.Pause.Disable();
    }

    void OnPauseOrResume(InputAction.CallbackContext context)
    {
        if (GameMaster.Instance.currentScene != "Main_Menu") {
            if (isPuased)
                Resume();
            else
                Pause();
        }
    }

    void Resume()
    {
        isPuased = false;

        // Handle cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Handle player control
        if (!DialogueManager.Instance.isTalking)
            Utility.EnablePlayerControl(true);
        SoundManager.Instance.UnpauseAll();

        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
    }

    void Pause()
    {
        isPuased = true;

        // Handle cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // Stop player control
        Utility.EnablePlayerControl(false);
        SoundManager.Instance.PauseAll();

        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        isPuased = false;
        pauseMenuUI.SetActive(false);
        GameMaster.Instance.RequestSceneChangeToMainMenu();
    }
}