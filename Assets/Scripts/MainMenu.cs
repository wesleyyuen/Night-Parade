using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button continueButton;
    public TextMeshProUGUI continueText;

    void Start () {
        SetUIHelper (false);
        if (SaveManager.Load (1) == null) { // TODO if File exists
            continueButton.interactable = false;
            continueText.color = Color.gray;
        } else {
            continueButton.interactable = true;
            continueText.color = Color.white;
        }
    }

    public void NewGame () {
        GameMaster gameMaster = FindObjectOfType<GameMaster> ();
        PlayerData playerData = new PlayerData (gameMaster.startingHealth, 0);
        gameMaster.RequestSceneChange ("Forest_Tutorial", playerData);
        SetUIHelper (true);
    }

    public void LoadGameFile (int index) {
        PlayerData playerData = SaveManager.Load (index);
        if (playerData == null) return;
        FindObjectOfType<GameMaster> ().savedPlayerData = playerData;
        SceneManager.LoadScene (playerData.LastSavePoint);

        SetUIHelper (true);
        PauseMenu.isPuased = false;
        GameObject pauseMenu = GameObject.FindGameObjectWithTag ("PauseMenu");
        if (pauseMenu != null) pauseMenu.SetActive (false);
    }

    public void QuitGame () {
        Debug.Log ("Quit!");
        Application.Quit ();
    }

    void SetUIHelper (bool boolean) {
        FindObjectOfType<CameraPeeking> ().enabled = boolean;
        FindObjectOfType<DialogueManager> ().enabled = boolean;
        GameObject.FindGameObjectWithTag ("MonUI").transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
        GameObject.FindGameObjectWithTag ("HealthUI").transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);

        GameObject dialogueUI = GameObject.FindGameObjectWithTag ("DialogueUI");
        if (dialogueUI != null) dialogueUI.transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
        GameObject pauseMenu = GameObject.FindGameObjectWithTag ("PauseMenu");
        if (pauseMenu != null) pauseMenu.transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
        GameObject pauseMenuOptions = GameObject.FindGameObjectWithTag ("PauseMenuOptions");
        if (pauseMenuOptions != null) pauseMenuOptions.transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
    }
}