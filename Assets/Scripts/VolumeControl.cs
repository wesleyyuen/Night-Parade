using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour {

    [SerializeField] private AudioMixer mixer;

    public void SetVolume (float volume) {
        mixer.SetFloat ("MasterVolume", volume);
    }
}