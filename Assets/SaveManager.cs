using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {
    private static SaveManager Instance;
    public int loadIndex;
    public string fileName;
    public string fileExtension;

    void Awake () {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad (gameObject);
        } else {
            Destroy (gameObject);
        }
        loadIndex = FindObjectOfType<GameMaster>().savedPlayerData.SaveFileIndex;
    }

    public static void Save (GameObject player) {
        BinaryFormatter formatter = new BinaryFormatter (); 

        string path = Application.persistentDataPath + Instance.fileName + Instance.loadIndex + Instance.fileExtension;
        if (File.Exists (path)) File.Delete (path); // TODO maybe overwrite instead of delete and create
        FileStream fileStream = new FileStream (path, FileMode.Create);

        PlayerData playerData = new PlayerData (player, SceneManager.GetActiveScene ().buildIndex, Instance.loadIndex);
        Debug.Log("Now Saving...");
        formatter.Serialize (fileStream, playerData);
        fileStream.Close ();
        Debug.Log("Saved!");
    }

    public static PlayerData Load (int index) {
        string path = Application.persistentDataPath + Instance.fileName + index + Instance.fileExtension;
        if (File.Exists (path)) {
            BinaryFormatter formatter = new BinaryFormatter ();
            FileStream fileStream = new FileStream (path, FileMode.Open);
            Debug.Log("Now Loading...");
            PlayerData playerData = formatter.Deserialize (fileStream) as PlayerData;
            fileStream.Close ();
            Instance.loadIndex = index;
            Debug.Log("Save Loaded!");
            return playerData;
        } else {
            Debug.Log ("Save File Not Found in " + path);
            return null;
        }
    }

    public static int GetLoadIndex() {
        return Instance.loadIndex;
    }
}