using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool isPuased = false;
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject optionsMenuUI;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        InputManager.Instance.Event_UIInput_Pause += OnPauseOrResume;
    }

    private void OnDisable()
    {
        InputManager.Instance.Event_UIInput_Pause -= OnPauseOrResume;
    }

    private void OnPauseOrResume()
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
        // InputManager.Instance.EnableGameplayInput();
        if (!DialogueManager.Instance.isTalking)
            Utility.EnablePlayerControl(true);
        AudioManager.Instance.UnpauseAll();

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
        // InputManager.Instance.EnableMenuInput();
        Utility.EnablePlayerControl(false);
        AudioManager.Instance.PauseAll();

        TimeManager.Instance.SetTimeScale(0f);
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void Continue()
    {
        Resume();
    }

    public void QuitToMainMenu()
    {
        TimeManager.Instance.SetTimeScale(1f);
        isPuased = false;
        pauseMenuUI.SetActive(false);
        GameMaster.Instance.RequestSceneChangeToMainMenu();
    }
}