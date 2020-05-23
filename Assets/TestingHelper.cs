using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingHelper : MonoBehaviour {

    void Awake () {
        GameMaster gameMaster = FindObjectOfType<GameMaster>();
        if (gameMaster == null) {
            SceneManager.LoadScene("_Preload");
        }
    }
}