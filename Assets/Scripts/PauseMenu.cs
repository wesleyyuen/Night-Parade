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
        if (!FindObjectOfType<DialogueManager> ().isTalking) {
            FindObjectOfType<PlayerCombat> ().enabled = true;
            FindObjectOfType<PlayerMovement> ().enabled = true;
        }

        Time.timeScale = 1f;
        pauseMenuUI.SetActive (false);
    }

    private void Pause () {
        isPuased = true;

        // Stop player control
        FindObjectOfType<PlayerCombat> ().enabled = false;
        FindObjectOfType<PlayerMovement> ().enabled = false;

        Time.timeScale = 0f;
        pauseMenuUI.SetActive (true);
    }

    public void QuitToMainMenu () {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive (false);
        SceneManager.LoadScene ("Main_Menu");
    }
}