using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using MEC;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance {
        get  {return instance; }
    }

    // private AudioSource _playerSource;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Sound[] sounds;
    

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
            s.source.outputAudioMixerGroup = _mixer.FindMatchingGroups("Master")[0];
            s.source.clip = s.clip[0];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    // private void OnEnable()
    // {
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    // }

    // private void OnDisable()
    // {
    //     SceneManager.sceneLoaded -= OnSceneLoaded;
    // }

    // // Update Player References
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     PlayerAudio player = FindObjectOfType<PlayerAudio>();
    //     if (player != null && player.TryGetComponent<AudioSource>(out AudioSource source))
    //         _playerSource = source;
    // }

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

    public void PlayOnce(string name, float volumeScale = 1f)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null) {
            Debug.Log("Sound " + name + " Not found in SoundManager Array");
            return;
        }

        if (sound.clip.Length > 1) { // Randomly choose a clip to play
            sound.source.clip = sound.clip[UnityEngine.Random.Range(0, sound.clip.Length)];
        }

        sound.source.PlayOneShot(sound.source.clip, sound.source.volume * volumeScale);
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

    private IEnumerator<float> _FadeVolume(Sound sound, float from, float to, float duration)
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