using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isPuased = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject optionsMenuUI;
    InputMaster _input;

    private void Awake()
    {
        _input = new InputMaster();

        _input.UI.Pause.performed += OnPauseOrResume;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _input.UI.Pause.Enable();
    }

    private void OnDisable()
    {
        _input.UI.Pause.Disable();
    }

    private void OnPauseOrResume(InputAction.CallbackContext context)
    {
        if (GameMaster.Instance.currentScene != "Main_Menu") {
            if (isPuased)
                Resume();
            else
                Pause();
        }
    }

    private void Resume()
    {
        isPuased = false;

        // Handle cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Handle player control
        if (!DialogueManager.Instance.isTalking)
            Utility.EnablePlayerControl(true);
        SoundManager.Instance.UnpauseAll();

        TimeManager.Instance.SetTimeScale(1f);
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
    }

    private void Pause()
    {
        isPuased = true;

        // Handle cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        // Stop player control
        Utility.EnablePlayerControl(false);
        SoundManager.Instance.PauseAll();

        TimeManager.Instance.SetTimeScale(0f);
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    private void QuitToMainMenu()
    {
        TimeManager.Instance.SetTimeScale(1f);
        isPuased = false;
        pauseMenuUI.SetActive(false);
        GameMaster.Instance.RequestSceneChangeToMainMenu();
    }
}