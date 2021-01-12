using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {
    private static AudioManager Instance;

    // AudioSource player;  // TODO maybe find the player's audio source and use it
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Sound[] sounds;
    

    void Awake () {
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad (gameObject); // Handled by Parent
        } else {
            Destroy (gameObject);
        }

        foreach (Sound s in sounds) {
            // Add a source for each sound
            s.source = gameObject.AddComponent<AudioSource> ();
            // TODO: Add a type field to Sound and add them to different group (music, SFX)
            s.source.outputAudioMixerGroup = mixer.FindMatchingGroups("Master")[0];
            s.source.clip = s.clip[0];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // TODO: better way of handling play than start and update; maybe in scene script
    void Start () {
        Play ("Forest_Ambience");
    }

    void Update () {
        if (SceneManager.GetActiveScene ().name.Contains ("Forest")) {
            ChangeVolume ("Forest_Ambience", 0.3f);
        } else {
            ChangeVolume ("Forest_Ambience", 0f);
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

    public void PauseAll () {
        foreach (Sound sound in sounds) {
            sound.source.Pause();
        }
    }

    public void UnpauseAll () {
        foreach (Sound sound in sounds) {
            sound.source.UnPause();
        }
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