using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPuased = false;
    [SerializeField] GameObject pauseMenuUI;

    void Update ()
    {
        if (SceneManager.GetActiveScene ().name != "Main_Menu" && Input.GetKeyDown (KeyCode.Escape)) {
            if (isPuased) Resume ();
            else Pause ();
        }
    }

    void Resume ()
    {
        isPuased = false;

        // Handle player control
        if (!DialogueManager.Instance.isTalking)
            Utility.EnablePlayerControl(true);
        SoundManager.Instance.UnpauseAll();

        Time.timeScale = 1f;
        pauseMenuUI.SetActive (false);
    }

    void Pause ()
    {
        isPuased = true;

        // Stop player control
        Utility.EnablePlayerControl(false);
        SoundManager.Instance.PauseAll();

        Time.timeScale = 0f;
        pauseMenuUI.SetActive (true);
    }

    void QuitToMainMenu ()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive (false);
        GameMaster.Instance.RequestSceneChangeToMainMenu();
    }
}