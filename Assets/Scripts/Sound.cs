using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {
    
    public string name;
    public AudioClip[] clip;
    [HideInInspector]
    public AudioSource source;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    // public bool fromPlayer; // TODO maybe instead of adding a source, see if the sourse is from the player, if yes, use the player's AudioSource
}