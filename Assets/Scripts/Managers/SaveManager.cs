using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    static SaveManager instance;
    public static SaveManager Instance {
        get  {return instance; }
    }
    [SerializeField] int loadIndex;
    [SerializeField] string fileName;
    [SerializeField] string fileExtension;

    void Awake ()
    {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad (gameObject); // Handled by Parent
        } else {
            Destroy (gameObject);
        }
        loadIndex = GameMaster.Instance.savedPlayerData.SaveFileIndex;
    }

    public static void Save(GameObject player)
    {
        BinaryFormatter formatter = new BinaryFormatter ();

        string path = Application.dataPath + Instance.fileName + Instance.loadIndex + Instance.fileExtension;
        if (File.Exists(path))
            File.Delete(path); // TODO maybe overwrite instead of delete and create
        FileStream fileStream = new FileStream (path, FileMode.Create);

        PlayerData playerData = new PlayerData(player, true, SceneManager.GetActiveScene().buildIndex, Instance.loadIndex);
        Debug.Log ("Now Saving...");
        formatter.Serialize (fileStream, playerData);
        fileStream.Close ();
        Debug.Log ("Saved!");
    }

    /*
         public static void SaveOnDeath (GameObject player, float percentOfCoinsLostAfterDeath) {
             BinaryFormatter formatter = new BinaryFormatter ();

            string path = Application.dataPath + Instance.fileName + Instance.loadIndex + Instance.fileExtension;
            if (File.Exists (path)) File.Delete (path); // TODO maybe overwrite instead of delete and create
            FileStream fileStream = new FileStream (path, FileMode.Create);

            PlayerData playerData = new PlayerData (player, percentOfCoinsLostAfterDeath, Instance.loadIndex);
            Debug.Log("Now Saving...");
            formatter.Serialize (fileStream, playerData);
            fileStream.Close ();
            Debug.Log("Saved!");
        }
        */

    public static bool HaveSaveData ()
    {
        string path1 = Application.dataPath + Instance.fileName + "1" + Instance.fileExtension;
        string path2 = Application.dataPath + Instance.fileName + "2" + Instance.fileExtension;
        string path3 = Application.dataPath + Instance.fileName + "3" + Instance.fileExtension;
        return File.Exists(path1) || File.Exists(path2) || File.Exists(path3);
    }

    public static PlayerData Load (int index)
    {
        string path = Application.dataPath + Instance.fileName + index + Instance.fileExtension;
        if (File.Exists (path)) {
            BinaryFormatter formatter = new BinaryFormatter ();
            FileStream fileStream = new FileStream (path, FileMode.Open);
            Debug.Log ("Now Loading...");
            // Load Save Data
            PlayerData playerData = formatter.Deserialize (fileStream) as PlayerData;
            fileStream.Close ();
            Instance.loadIndex = index;
            Debug.Log ("Save Loaded!");
            return playerData;
        } else {
            Debug.Log ("Save File Not Found in " + path);
            return null;
        }
    }

    public static int GetLoadIndex ()
    {
        return Instance.loadIndex;
    }
}