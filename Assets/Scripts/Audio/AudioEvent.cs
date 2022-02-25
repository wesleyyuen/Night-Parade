using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioEvent : ScriptableObject
{
    public AudioType type;
    public abstract void Play(AudioSource source = null);
}
