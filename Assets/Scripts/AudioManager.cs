using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    // AudioSource player;  // TODO maybe find the player's audio source and use it
    [SerializeField] private Sound[] sounds;

    void Awake () {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource> ();
            s.source.clip = s.clip[0];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start () {
        Play ("Forest_Ambience");
    }

    void Update () {
        if (SceneManager.GetActiveScene().name == "Main_Menu") {
            ChangeVolume ("Forest_Ambience", 0f);
        } else if (SceneManager.GetActiveScene().name.Contains("Forest")) {
            ChangeVolume ("Forest_Ambience", 0.3f);
        }
    }

    public void Play (string name) {
        Sound sound = Array.Find (sounds, s => s.name == name);
        if (sound == null) {
            Debug.Log ("Sound " + name + " Not found in AudioManager Array");
            return;
        }

        if (sound.clip.Length > 1) { // Randomly choose a clip to play
            sound.source.clip = sound.clip[UnityEngine.Random.Range (0, sound.clip.Length)];
        }
        sound.source.Play ();
    }

    public void ChangeVolume (string name, float volume) {
        Sound sound = Array.Find (sounds, s => s.name == name);
        if (sound == null) {
            Debug.Log ("Sound " + name + " Not found in AudioManager Array");
            return;
        }
        sound.source.volume = volume;
    }
}