using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;
    public TextMeshProUGUI continueText;

    void Start ()
    {
        if (!SaveManager.HaveSaveData()) { // TODO if File exists
            continueButton.interactable = false;
            continueText.color = Color.gray;
        } else {
            continueButton.interactable = true;
            continueText.color = Color.white;
        }

        GameMaster.Instance.UpdateCurrentScene ();
    }

    public void NewGame ()
    {
        PlayerData playerData = new PlayerData (GameMaster.Instance.startingHearts * 4, GameMaster.Instance.startingStamina);
        GameMaster.Instance.RequestSceneChange ("Forest_Cave", ref playerData);
    }

    public void LoadGameFile (int index)
    {
        PlayerData playerData = SaveManager.Load (index);
        if (playerData == null) return;
        GameMaster.Instance.savedPlayerData = playerData;
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(playerData.LastSavePoint));
        GameMaster.Instance.RequestSceneChange (sceneName, ref playerData);

        // Handle Pause Menu (when player pause to quit to main menu and load game)
        PauseMenu.isPuased = false;
        GameObject pauseMenu = GameObject.FindGameObjectWithTag ("PauseMenu");
        if (pauseMenu != null) pauseMenu.SetActive (false);
    }

    public void QuitGame ()
    {
        Debug.Log ("Quit!");
        Application.Quit ();
    }
}