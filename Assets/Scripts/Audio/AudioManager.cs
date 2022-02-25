using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using MEC;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance {
        get  {return instance; }
    }
    [SerializeField] private AudioMixer _mixer;
    private List<AudioSource> soundtrackAudioSources = new List<AudioSource>();
    private List<AudioSource> ambientAudioSources = new List<AudioSource>();
    private List<AudioSource> sfxAudioSources = new List<AudioSource>();
    
    private void Awake()
    {
        if (instance == null) {
            instance = this;
            //DontDestroyOnLoad (gameObject); // Handled by Parent
        } else {
            Destroy (gameObject);
        }

        for (int i = 0; i < 2; ++i) {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            soundtrackAudioSources.Add(s);
            s.outputAudioMixerGroup = _mixer.FindMatchingGroups("Soundtrack")[0];
        }

        for (int i = 0; i < 3; ++i) {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            ambientAudioSources.Add(s);
            s.outputAudioMixerGroup = _mixer.FindMatchingGroups("Ambient")[0];
        }

        for (int i = 0; i < 5; ++i) {
            AudioSource s = gameObject.AddComponent<AudioSource>();
            sfxAudioSources.Add(s);
            s.outputAudioMixerGroup = _mixer.FindMatchingGroups("SFX")[0];
        }
    }

    public AudioSource GetSoundtrackAudioSource()
    {
        foreach (AudioSource s in soundtrackAudioSources) {
            if (!s.isPlaying) return s;
        }

        return soundtrackAudioSources[0];
    }

    public AudioSource GetAmbientAudioSource()
    {
        foreach (AudioSource s in soundtrackAudioSources) {
            if (!s.isPlaying) return s;
        }

        return soundtrackAudioSources[0];
    }

    public AudioSource GetSFXAudioSource()
    {
        foreach (AudioSource s in sfxAudioSources) {
            if (!s.isPlaying) return s;
        }

        return sfxAudioSources[0];
    }

    public void PauseAll()
    {
        AudioListener.pause = true;
    }

    public void UnpauseAll()
    {
        AudioListener.pause = false;
    }

    public void SetMasterVolume(float volume)
    {
        _mixer.SetFloat("MasterVolume", volume);
    }

    public void SetSoundtrackVolume(float volume)
    {
        _mixer.SetFloat("SoundtrackVolume", volume);
    }

    public void SetAmbientVolume(float volume)
    {
        _mixer.SetFloat("AmbientVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        _mixer.SetFloat("SFXVolume", volume);
    }
}