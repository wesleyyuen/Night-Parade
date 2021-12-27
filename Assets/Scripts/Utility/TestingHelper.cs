using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingHelper : MonoBehaviour
{
    // Make sure _Preload is loaded before anything, help with playmode editor
    private void Awake()
    {
        if (GameMaster.Instance == null) {
            SceneManager.LoadScene("_Preload");
        }
    }
}