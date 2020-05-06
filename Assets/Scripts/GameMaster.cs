using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
    private static GameMaster Instance;
    public static string prevScene = "";
    public static string currentScene = "";
    public int savedPlayerHealth = 3;
    
    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (Instance);
        } else {
            Destroy (gameObject);
        }
        UpdateCurrentScene ();
    }

    public void UpdateCurrentScene () {
        currentScene = SceneManager.GetActiveScene ().name;
    }

    public string getPrevScene () {
        return prevScene;
    }

    public void requestSceneChange (string sceneToLoad, int currHealth) {
        prevScene = currentScene;
        savedPlayerHealth = currHealth;
        SceneManager.LoadScene (sceneToLoad);
    }
}