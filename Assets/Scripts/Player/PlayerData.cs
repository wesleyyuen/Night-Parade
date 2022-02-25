using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int SaveFileIndex { get; private set; }
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public int CoinsOnHand { get; private set; }
    public int LastSavePoint { get; private set; }
    public float PlayTime { get; private set; } // In Seconds
    public Dictionary<string, SceneData> SceneData { get; private set; }

    // Empty
    public PlayerData()
    {
        SaveFileIndex = 0;
    }

    // For Loading inbetween sessions (Continue Game)
    public PlayerData(
        int saveFileIndex,
        int maxHealth,
        int coinsOnHand,
        int lastSavePoint,
        float playTime,
        Dictionary<string, SceneData> sceneData
    )
    {
        SaveFileIndex = saveFileIndex;
        CurrentHealth = maxHealth;
        MaxHealth = maxHealth;
        CoinsOnHand = coinsOnHand;
        LastSavePoint = lastSavePoint;
        PlayTime = playTime;
        SceneData = sceneData;
    }

    // For Saving (both inbetween and within session) and Loading within session
    public PlayerData(GameObject player, bool hardSave, int sceneIndex, int loadIndex)
    {
        SaveFileIndex = loadIndex;

        if (player.TryGetComponent<PlayerHealthMO>(out PlayerHealthMO health)) {
            CurrentHealth = health.currHealth;
            MaxHealth = health.maxHealth;
        }

        PlayTime = SaveManager.Instance.GetPlayTimeInScene();
        SceneData = SaveManager.Instance.savedSceneData;

        if (player.TryGetComponent<PlayerInventory>(out PlayerInventory inventory)) {
            CoinsOnHand = inventory.MonOnHand;
        }
        
        if (hardSave) {
            // change save point to current scene if manual saving i.e. saving at SavePoint
            LastSavePoint = sceneIndex;
        }
    }

    public bool IsValid()
    {
        return SaveFileIndex != 0;
    }

    public void Invalidate()
    {
        SaveFileIndex = 0;
    }

    public void Reset()
    {

    }
}