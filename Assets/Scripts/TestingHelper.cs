using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingHelper : MonoBehaviour {
    // Make sure _Preload is loaded before anything, help with playmode editor

    void Awake () {
        GameMaster gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null) {
            SceneManager.LoadScene("_Preload");
        }
    }
}