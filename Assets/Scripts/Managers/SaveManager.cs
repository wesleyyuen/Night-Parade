using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    static SaveManager _instance;
    public static SaveManager Instance {
        get  {return _instance; }
    }
    [SerializeField] string fileName;
    [SerializeField] string fileExtension;
    [HideInInspector] public PlayerData savedPlayerData;
    [HideInInspector] public Dictionary<string, SceneData> savedSceneData;

    private void Awake()
    {
        if (_instance == null) {
            _instance = this;
            //DontDestroyOnLoad (gameObject); // Handled by Parent
        } else {
            Destroy (gameObject);
        }

        InitializeEmptySaveData();
    }

    private void OnEnable()
    {
        GameMaster.Event_GameMasterInitalized += LoadStartingScene;
    }

    private void OnDisable()
    {
        GameMaster.Event_GameMasterInitalized -= LoadStartingScene;
    }

    private void LoadStartingScene()
    {
        GameMaster.Instance.RequestSceneChange(GameMaster.Instance.startingScene.ToString(), ref savedPlayerData);
    }

    private void InitializeEmptySaveData()
    {
        Dictionary<string, SceneData> scenes = new Dictionary<string, SceneData>();
        scenes.Add("Overall", new SceneData("Overall", new Dictionary<string, int>()));

        savedPlayerData = new PlayerData(
            saveFileIndex: 1,
            maxHealth: Constant.STARTING_HEARTS * 4,
            coinsOnHand: 0,
            lastSavePoint: 0,
            playTime: 0f,
            sceneData: scenes
        );
        savedSceneData = savedPlayerData.SceneData;
    }

    public static bool Save(GameObject player)
    {
        int saveSlot = SaveManager.Instance.savedPlayerData.SaveFileIndex;

        string path = Application.dataPath + Instance.fileName + saveSlot + Instance.fileExtension;
        if (File.Exists(path))
            File.Delete(path); // TODO maybe overwrite instead of delete and create

        PlayerData data = new PlayerData(player, true, SceneManager.GetActiveScene().buildIndex, saveSlot);
        try
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate))) {
                writer.Write(data.SaveFileIndex);
                writer.Write(data.MaxHealth);
                writer.Write(data.CoinsOnHand);
                writer.Write(data.LastSavePoint);
                writer.Write(data.PlayTime);

                writer.Write(data.SceneData.Count);
                foreach (KeyValuePair<string, SceneData> entry1 in data.SceneData) {
                    writer.Write(entry1.Key);

                    writer.Write(entry1.Value.PermaProgress.Count);
                    foreach (KeyValuePair<string, int> entry2 in entry1.Value.PermaProgress) {
                        writer.Write(entry2.Key);
                        writer.Write(entry2.Value);
                    }
                }

                Debug.Log ("Save Successfully!");
                return true;
            }
        } catch (IOException e) {
            Debug.LogException(e);
            Debug.Log("Save Failed!");
            return false;
        }
    }

    public bool HaveSaveData()
    {
        string path1 = Application.dataPath + Instance.fileName + "1" + Instance.fileExtension;
        string path2 = Application.dataPath + Instance.fileName + "2" + Instance.fileExtension;
        string path3 = Application.dataPath + Instance.fileName + "3" + Instance.fileExtension;
        return File.Exists(path1) || File.Exists(path2) || File.Exists(path3);
    }

    public static PlayerData Load(int index)
    {
        string path = Application.dataPath + Instance.fileName + index + Instance.fileExtension;
        if (File.Exists(path)) {
            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open))) {
                int _saveFileIndex = reader.ReadInt32();
                int _maxHealth = reader.ReadInt32();
                int _coinsOnHand = reader.ReadInt32();
                int _lastSavePoint = reader.ReadInt32();
                float _playTime = reader.ReadSingle();

                int _sceneProgressCount = reader.ReadInt32();
                Dictionary<string, SceneData> _sceneData = new Dictionary<string, SceneData>();
                for (int i = 0; i < _sceneProgressCount; ++i) {
                    string _sceneName = reader.ReadString();
                    int _count = reader.ReadInt32();

                    Dictionary<string, int> perma = new Dictionary<string, int>();
                    for (int j = 0; j < _count; ++j) {
                        string _key = reader.ReadString();
                        int _val = reader.ReadInt32(); 
                        perma.Add(_key, _val);
                    }
                    SceneData progress = new SceneData(_sceneName, perma);
                    _sceneData.Add(_sceneName, progress);
                }

                PlayerData data = new PlayerData(
                    saveFileIndex: _saveFileIndex,
                    maxHealth: _maxHealth,
                    coinsOnHand: _coinsOnHand,
                    lastSavePoint: _lastSavePoint,
                    playTime: _playTime,
                    sceneData: _sceneData
                );

                Debug.Log("Save File Found and Successfully Loaded!");
                return data;
            }
        } else {
            Debug.Log("Save File Not Found in " + path);
            return null;
        }
    }

    public int GetLoadIndex()
    {
        return SaveManager.Instance.savedPlayerData.SaveFileIndex;
    }

    #region Player Progress
    public float GetPlayTimeInScene()
    {
        return savedPlayerData.PlayTime + Time.timeSinceLevelLoad;
    }
    
    public SceneData GetSceneData(string name)
    {
        if (savedSceneData.ContainsKey(name))
            return savedSceneData[name];
        else {
            // Add to
            SceneData data = new SceneData(name, new Dictionary<string, int>());
            savedSceneData[name] = data;
            return data;
        }
    }
 
    public bool HasOverallProgress(string key)
    {
        return savedSceneData["Overall"].PermaProgress.ContainsKey(key);
    }

    public bool HasScenePermaProgress(string sceneName, string key)
    {
        SceneData data = GetSceneData(sceneName);
        return data.PermaProgress.ContainsKey(key);
    }

    public void AddOverallProgress(string key, int value)
    {
        SceneData data = savedSceneData["Overall"];
        data.PermaProgress[key] = value;
    }

    public void AddScenePermaProgress(string sceneName, string key, int value)
    {
        SceneData data = GetSceneData(sceneName);
        data.PermaProgress[key] = value;
    }

    public void UpdateSpawnTimestamp(string name, float time)
    {
        savedSceneData[GameMaster.Instance.currentScene].EnemySpawnTimestamps[name] = time;
    }

    #endregion
}