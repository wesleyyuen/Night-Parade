using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;
    public TextMeshProUGUI continueText;

    private void Start()
    {
        if (!SaveManager.Instance.HaveSaveData()) { // TODO if File exists
            continueButton.interactable = false;
            continueText.color = Color.gray;
        } else {
            continueButton.interactable = true;
            continueText.color = Color.white;
        }
    }

    public void NewGame()
    {
        Dictionary<string, SceneData> scenes = new Dictionary<string, SceneData>();
        scenes.Add("Overall", new SceneData("Overall", new Dictionary<string, int>()));

        PlayerData playerData = new PlayerData(
            saveFileIndex: 1,
            maxHealth: Constant.STARTING_HEARTS * 4,
            maxStamina: Constant.STARTING_STAMINA,
            coinsOnHand: 0,
            lastSavePoint: 0,
            playTime: 0f,
            savedInks: new bool[Constant.NUMBER_OF_AREAS],
            savedOrbs: 0,
            sceneData: scenes 
        );

        GameMaster.Instance.RequestSceneChange(Constant.SceneName.Forest_Cave.ToString(), ref playerData);
    }

    public void LoadGameFile(int index)
    {
        PlayerData playerData = SaveManager.Load(index);
        if (playerData == null) {
            Debug.Log("Load Failed!");
            return;
        }
        SaveManager.Instance.savedPlayerData = playerData;
        SaveManager.Instance.savedSceneData = playerData.SceneData;
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(playerData.LastSavePoint));
        GameMaster.Instance.RequestSceneChange(sceneName, ref playerData);
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}