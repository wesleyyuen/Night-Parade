using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using MEC;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;
    public static SoundManager Instance {
        get  {return instance; }
    }

    // AudioSource player;  // TODO maybe find the player's audio source and use it
    [SerializeField] AudioMixer mixer;
    [SerializeField] Sound[] sounds;
    

    private void Awake()
    {
        if (instance == null) {
            instance = this;
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

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null) {
            Debug.Log("Sound " + name + " Not found in SoundManager Array");
            return;
        }

        if (sound.clip.Length > 1) { // Randomly choose a clip to play
            sound.source.clip = sound.clip[UnityEngine.Random.Range(0, sound.clip.Length)];
        }

        sound.source.Play();
    }

    public void FadeIn(string name, float fadeDuration)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null) {
            Debug.Log("Sound " + name + " Not found in SoundManager Array");
            return;
        }

        if (sound.clip.Length > 1) { // Randomly choose a clip to play
            sound.source.clip = sound.clip[UnityEngine.Random.Range(0, sound.clip.Length)];
        }

        // sound.source.volume = 0f;
        // sound.source.DOFade(sound.source.volume, fadeDuration);

        Timing.RunCoroutine(_FadeVolume(sound, 0f, sound.source.volume, fadeDuration));
        sound.source.Play();
    }

    IEnumerator<float> _FadeVolume(Sound sound, float from, float to, float duration)
    {
        sound.source.volume = from;
        for (float t = 0f; t < 1f; t += Time.deltaTime / duration) {
            sound.source.volume = Mathf.SmoothStep(from, to, t);
            yield return Timing.WaitForOneFrame;
        }
        sound.source.volume = to;
    }

    public void PauseAll()
    {
        foreach (Sound sound in sounds) {
            sound.source.Pause();
        }
    }

    public void UnpauseAll()
    {
        foreach (Sound sound in sounds) {
            sound.source.UnPause();
        }
    }

    public void ChangeVolume(string name, float volume)
    {
        Sound sound = Array.Find (sounds, s => s.name == name);
        if (sound == null) {
            Debug.Log ("Sound " + name + " Not found in SoundManager Array");
            return;
        }
        sound.source.volume = volume;
    }
}