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

    public void Resume () {
        isPuased = false;
        if (!FindObjectOfType<DialogueManager> ().isTalking) {
            FindObjectOfType<PlayerCombat> ().enabled = true;
            FindObjectOfType<PlayerMovement> ().enabled = true;
        }

        Time.timeScale = 1f;
        pauseMenuUI.SetActive (false);
    }

    void Pause () {
        isPuased = true;
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