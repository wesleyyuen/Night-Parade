using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster Instance;
    public int savedPlayerHealth = 3;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void requestSceneChange(string sceneToLoad, int currHealth) {
        savedPlayerHealth = currHealth;
        SceneManager.LoadScene(sceneToLoad);
    }
}
