using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData
{
    public string SceneName;
    // Data that persist between sessions
    public Dictionary<string, int> PermaProgress;

    // Data that persist within sessions
    public Dictionary<string, float> EnemySpawnTimestamps;

    public SceneData(string sceneName, Dictionary<string, int> savedProgress)
    {
        SceneName = sceneName;
        PermaProgress = savedProgress;
        EnemySpawnTimestamps = new Dictionary<string, float>();
    }
}
