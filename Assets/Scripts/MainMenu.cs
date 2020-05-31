using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour {

    public Button continueButton;
    public TextMeshProUGUI continueText;

    void Start () {
        if (SaveManager.Load(1) == null) { // TODO if File exists
            continueButton.interactable = false;
            continueText.color = Color.gray;
        } else {
            continueButton.interactable = true;
            continueText.color = Color.white;
        }
    }

    public void NewGame () {
        SceneManager.LoadScene ("Forest_Sacred");
        SetUIHelper (true);
    }

    public void LoadGameFile (int index) {
        PlayerData playerData = SaveManager.Load (index);
        if (playerData == null) return;
        FindObjectOfType<GameMaster>().savedPlayerData = playerData;
        SceneManager.LoadScene (playerData.LastSavePoint);
        SetUIHelper (true);
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
        GameObject.FindGameObjectWithTag ("DialogueUI").transform.localScale = new Vector3 ((boolean) ? 1f : 0f, 1f, 0f);
    }
}