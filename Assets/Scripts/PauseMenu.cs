using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool isPuased = false;
    [SerializeField] private GameObject pauseMenuUI;

    void Update () {
        if (SceneManager.GetActiveScene ().name != "Main_Menu" && Input.GetKeyDown (KeyCode.Escape)) {
            if (isPuased) Resume ();
            else Pause ();
        }
    }

    private void Resume () {
        isPuased = false;

        // Handle player control
        if (!DialogueManager.Instance.isTalking)
            Common.EnablePlayerControl(true);
        AudioManager.Instance.UnpauseAll();

        Time.timeScale = 1f;
        pauseMenuUI.SetActive (false);
    }

    private void Pause () {
        isPuased = true;

        // Stop player control
        Common.EnablePlayerControl(false);
        AudioManager.Instance.PauseAll();

        Time.timeScale = 0f;
        pauseMenuUI.SetActive (true);
    }

    private void QuitToMainMenu () {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive (false);
        SceneManager.LoadScene ("Main_Menu");
    }
}