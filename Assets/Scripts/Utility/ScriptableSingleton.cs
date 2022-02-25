using System.Linq;
using UnityEngine;

public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableSingleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
		{
			if (!_instance) {
				_instance = Resources.LoadAll<T>("").FirstOrDefault();
			}
			
#if UNITY_EDITOR
			if (!_instance) {
				InitializeFromDefault(UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets/Test game settings.asset"));
			}
#endif
			return _instance;
		}
    }

	public static void InitializeFromDefault(T singleton)
	{
		if (_instance) DestroyImmediate(_instance);
		_instance = Instantiate(singleton);
		_instance.hideFlags = HideFlags.HideAndDontSave;
	}

	// public static void LoadFromJSON(string path)
	// {
	// 	if (!_instance) DestroyImmediate(_instance);
	// 	_instance = ScriptableObject.CreateInstance<T>();
	// 	JsonUtility.FromJsonOverwrite(System.IO.File.ReadAllText(path), _instance);
	// 	_instance.hideFlags = HideFlags.HideAndDontSave;
	// }

	// public void SaveToJSON(string path)
	// {
	// 	Debug.LogFormat("Saving game settings to {0}", path);
	// 	System.IO.File.WriteAllText(path, JsonUtility.ToJson(this, true));
	// }

}
